using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    None = -1,
    Idle,
    Walk
}

public class CharacterFSM : MonoBehaviour
{
    private CharacterState state;

    private Animator anim;
    private CharacterCustomization cc;

    private float speed = 1.2f;

    private void Start()
    {
        state = CharacterState.Walk;

        anim = GetComponent<Animator>();

        cc = GetComponent<CharacterCustomization>();
        cc.SetHeadByIndex(Random.Range(0, cc.headsPresets.Count));
        cc.SetElementByIndex(CharacterCustomization.ClothesPartType.Hat, Random.Range(-1, cc.hatsPresets.Count));
        cc.SetElementByIndex(CharacterCustomization.ClothesPartType.Accessory, Random.Range(-1, cc.accessoryPresets.Count));
        cc.SetElementByIndex(CharacterCustomization.ClothesPartType.TShirt, Random.Range(0, cc.shirtsPresets.Count));
        cc.SetElementByIndex(CharacterCustomization.ClothesPartType.Pants, Random.Range(0, cc.pantsPresets.Count));
        cc.SetElementByIndex(CharacterCustomization.ClothesPartType.Shoes, Random.Range(0, cc.shoesPresets.Count));
    }

    private void Update()
    {
        switch (state)
        {
            case CharacterState.None:
                break;
            case CharacterState.Idle:
                anim.Play("Idle");
                break;
            case CharacterState.Walk:
                transform.position += transform.rotation * Vector3.forward * speed * Time.deltaTime;
                Ray ray = new Ray(transform.position, transform.rotation * Vector3.forward);
                //if (Physics.Raycast(ray, 5.0f, )
                //{

                //}
                anim.SetBool("walk", true);
                anim.Play("walk");
                break;
            default:
                break;
        }
    }
}
