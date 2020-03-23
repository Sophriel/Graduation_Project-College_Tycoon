﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#region Enums

public enum CharacterState
{
    None = -1,
    Idle,
    Walk
}

public enum PathFindMethod
{
    BFS = 0,
    Astar
}

#endregion

public class CharacterFSM : MonoBehaviour
{
    public CharacterState State;
    public PathFindMethod Method;

    private Animator anim;
    private CharacterCustomization cc;

    public Dictionary<Vector3, PathNode> Nodes;
    public List<Vector3> Keys;
    public Stack<Vector3> Paths;
    public GameObject Destination;
    public int DirectionCount = 6;  //  각 노드당 탐색 방향
    public float DistanceBetweenNodes = 7.0f;  //  각 노드당 탐색거리

    private readonly float speed = 4.0f;  //  진행 속도

    private void Start()
    {
        State = CharacterState.Idle;

        anim = GetComponent<Animator>();

        cc = GetComponent<CharacterCustomization>();
        cc.SetHeadByIndex(Random.Range(0, cc.headsPresets.Count));
        cc.SetElementByIndex(CharacterCustomization.ClothesPartType.Hat, Random.Range(-1, cc.hatsPresets.Count));
        cc.SetElementByIndex(CharacterCustomization.ClothesPartType.Accessory, Random.Range(-1, cc.accessoryPresets.Count));
        cc.SetElementByIndex(CharacterCustomization.ClothesPartType.TShirt, Random.Range(0, cc.shirtsPresets.Count));
        cc.SetElementByIndex(CharacterCustomization.ClothesPartType.Pants, Random.Range(0, cc.pantsPresets.Count));
        cc.SetElementByIndex(CharacterCustomization.ClothesPartType.Shoes, Random.Range(0, cc.shoesPresets.Count));

        Nodes = new Dictionary<Vector3, PathNode>();  //  경로 탐색용 노드 Dictionary
        Keys = new List<Vector3>(Nodes.Keys);  //  노드 key값 저장 List
        Paths = new Stack<Vector3>();  //  확정된 경로 Stack
	}

	private void Update()
    {
        switch (State)
        {
            case CharacterState.None:
                break;
            case CharacterState.Idle:
                //if (!CastToDirection(Vector3.forward))
                //    State = CharacterState.Walk;

                //  경로 미설정시 재탐색
                if (!Destination)
                    SetTarget(GameManager.Instance.GetRandomBuildingInGame());

				anim.SetBool("walk", false);
				anim.Play("Idle");
				break;
            case CharacterState.Walk:
                //if (CastToDirection(Vector3.forward))
                //    State = CharacterState.Idle;

                anim.SetBool("walk", true);
                anim.Play("walk");
                break;
            default:
                break;
        }
    }

    //private bool CastToDirection(Vector3 dir)
    //{
    //    int layermask = 1 << LayerMask.NameToLayer("Building");

    //    bool hit = Physics.SphereCast(transform.position, 0.5f, Vector3.forward, out RaycastHit hitInfo, 5.0f, layermask);

    //    if (hit)
    //    {
    //        Debug.DrawRay(transform.position + Vector3.up, dir * 5.0f, Color.red, 3.0f);
    //        return true;
    //    }

    //    return false;
    //}

    #region 경로 탐색 알고리즘

    /// <summary>
    /// 목적지를 설정합니다.
    /// </summary>
    /// <param name="dest">목적지로 설정할 GameObject</param>
    private void SetTarget(GameObject dest)
    {
        if (!dest)
            return;

        Destination = dest;

        TrackTarget();
    }

    /// <summary>
    /// 경로 탐색 후 추적합니다.
    /// </summary>
    public void TrackTarget()
    {
        StopCoroutine("FollowPath");
        MouseManager.Instance.DeleteBuildEvent(TrackTarget);

        switch (Method)
        {
            case PathFindMethod.BFS:
                if (FindPathWithBFS())
                    StartCoroutine("FollowPath");

                else
                {
                    MouseManager.Instance.AddBuildEvent(TrackTarget);
                    State = CharacterState.Idle;
                }

                break;
            case PathFindMethod.Astar:
                if (FindPathWithAstar())
                    StartCoroutine("FollowPath");

                else
                {
                    MouseManager.Instance.AddBuildEvent(TrackTarget);
                    State = CharacterState.Idle;
                }

                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Destination까지 경로를 BFS 탐색하여 Paths에 저장합니다
    /// </summary>
    private bool FindPathWithBFS()
    {
        //  목적지 미할당시
        if (!Destination || !Destination.activeInHierarchy)
        {
            Debug.Log(name + "has no Destination");
            return false;
        }

        PathNode RootNode;
        ClearAll();

        //  루트노드 초기화
        RootNode = new PathNode(this, null, transform.position);

        //  키값 리스트를 구성하여 
        Nodes.Add(RootNode.pos, RootNode);
        Keys.Add(RootNode.pos);

        int i = 0;
        while (Nodes.Count > 0)
        {
            if (Nodes.Count > 100000)
            {
                Debug.Log(Nodes.Count + " " + name + " Failed to Find");
                return false;
            }

            if (Nodes[Keys[i]].FindPath())
                break;

            i++;
        }

        Debug.Log(name + " Nodes: " + Nodes.Count);
        Debug.Log(name + "Paths: " + Paths.Count);

        Nodes.Clear();
        Keys.Clear();
        GC.Collect();

        return true;
    }

    /// <summary>
    /// Destination까지 경로를 Astar 탐색하여 Paths에 저장합니다
    /// </summary>
    private bool FindPathWithAstar()
    {
        return false;
    }

    /// <summary>
    /// 설정된 Paths를 따라갑니다.
    /// </summary>
    private IEnumerator FollowPath()
    {
        MouseManager.Instance.AddBuildEvent(TrackTarget);  //  경로에 도달할 때 까지 Observe
        State = CharacterState.Walk;

        while (Paths.Count > 0)
        {
            Vector3 nextPath = Paths.Pop();
            transform.LookAt(nextPath);

            while (transform.position != nextPath && State == CharacterState.Walk)
            {
                Debug.DrawLine(transform.position, nextPath, Color.blue, 0.1f);
                transform.position = Vector3.MoveTowards(transform.position, nextPath, speed * Time.deltaTime);
                yield return null;
            }
        }

        Destination = null;
        State = CharacterState.Idle;
        MouseManager.Instance.DeleteBuildEvent(TrackTarget);
    }

    /// <summary>
    /// 모든 Collection을 정리합니다.
    /// </summary>
    private void ClearAll()
    {
        Nodes.Clear();
        Keys.Clear();
        Paths.Clear();
        GC.Collect();
    }

    #endregion
}

#region PathNode Class

public class PathNode
{
    public CharacterFSM Character;
    private GameObject destination;

    public PathNode parent;
    public Vector3 pos;

    private int dir;
    private float dist;
    private readonly int layermask = 1 << LayerMask.NameToLayer("Building");

    public PathNode(CharacterFSM whichCharacter, PathNode parentNode, Vector3 position)
    {
        Character = whichCharacter;
        destination = Character.Destination;
        parent = parentNode;
        pos = position;
        dir = Character.DirectionCount;
        dist = Character.DistanceBetweenNodes;
    }

    /// <summary>
    /// dir방향으로 dist거리 SphereCast를 실행
    /// </summary>
    public bool FindPath()
    {
        //  n방향 탐색
        for (int i = 0; i < dir; i++)
        {
            Ray ray = new Ray(pos + Vector3.up, Quaternion.Euler(0f, (360.0f / dir) * i, 0f) * Vector3.forward);
            bool hit = Physics.SphereCast(ray, 0.1f, out RaycastHit hitInfo, dist, layermask);

            Debug.DrawRay(pos + Vector3.up, 
                Quaternion.Euler(0f, (360.0f / dir) * i, 0f) * Vector3.forward * dist, Color.red, 2.0f);

            //  목적지 발견
            if (hit && hitInfo.transform.gameObject == destination)
            {
                Character.Paths.Push(hitInfo.transform.position);
                CompletePathFind();
                return true;
            }

            //  경로가 확정되면 나머지 노드는 경로찾기 중지
            if (Character.Paths.Count > 0)
                return false;

            //  충돌체 없을시 해당 위치에 노드 생성
            if (!hit)
            {
                PathNode node = new PathNode(Character, this, ray.GetPoint(dist) + Vector3.down);

                if (!Character.Nodes.ContainsKey(node.pos))
                {
                    Character.Nodes.Add(node.pos, node);
                    Character.Keys.Add(node.pos);
                }
            }
        }

        return false;
    }

    public void CompletePathFind()
    {
        Character.Paths.Push(pos);
        if (parent != null)
            parent.CompletePathFind();
    }
}

#endregion

