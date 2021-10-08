﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour {
    //스파크 프리팹을 저장할 변수
    public GameObject sparkEffect;

    //충돌이 시작할때 발생하는 이벤트
    private void OnCollisionEnter(Collision coll)
    {
        //충돌한 게임 오브젝트의 태그를 비교
        if(coll.collider.tag == "BULLET")
        {

            //스파크함수효과 호출
            ShowEffect(coll);
            //충돌한 게임오브젝트 삭제
            Destroy(coll.gameObject); // 너죽고
            //Destroy(gameObject);//나죽고
        }
    }

    void ShowEffect(Collision coll)
    {
        //충돌 지점의 정보를 추출
        ContactPoint contact = coll.contacts[0];
        //법선벡터가 이루는 회전 각도를 추출
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, -contact.normal);
        
        //스파크효과를 생성
        GameObject spark = Instantiate(sparkEffect, contact.point +(-contact.normal * 0.05f), rot);
        //스파크 효과를 부모를 드럼통 또는 벽으로 설정
        spark.transform.SetParent(this.transform);
    }

}
