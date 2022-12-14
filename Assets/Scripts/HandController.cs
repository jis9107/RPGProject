using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    // 현재 장착된 Hand형 타입 무기
    [SerializeField]
    private Hand currentHand;

    // 공격중??
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;


    // Update is called once per frame
    void Update()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isAttack)
            {
                // 코루틴 실행
                StartCoroutine(AttackCoroutine());

            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        StartCoroutine(HitCoroutine());
        // 공격 활성화 시점

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {   
                if(hitInfo.transform.tag == "Rock")
                {
                    hitInfo.transform.GetComponent<Rock>().Mining();
                }
                isSwing = false;
                //충돌 됨
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    private bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }
}
