using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//총알의 발사와 재장전 오디오 클립을 저장 구조체
//Inspector에 노출시키기위해 사용
[System.Serializable]
public struct PlayerSfx
{
    //여기서 배열 요소 값들을 지정하지 않고 유니티에서 지정
    public AudioClip[] fire;
    public AudioClip[] reload;
}

public class FireCtrl : MonoBehaviour {
    //무기타입, enum->인스펙터뷰에 콤보박스 형태로 나타남
    public enum WeaponType
    {
        RIFLE=0,
        SHOTGUN
    }
    //주인공이 현재 들고 있는 무기를 저장할 변수
    public WeaponType currWeapon = WeaponType.RIFLE;

    //총알 프리팹 변수
    public GameObject bullet;
    // 총알 발사 좌표
    public Transform firePos;
    //탄피 추출 파티클
    public ParticleSystem cartridge;
    //총구 화염 파티클
    private ParticleSystem muzzleFlash;
    //AUDIOSOURCE 컴포넌트를 저장할 변수
    private AudioSource _audio;

    //오디오클립을 저장할 변수
    //PlayerSfx.fire[배열], PlayerSfx.reload[배열] 형식으로 사용
    public PlayerSfx playerSfx;

    //Shake 클래스를 저장할 변수
    private Shake shake;
    

    // Use this for initialization
    void Start () {
        //FirePos하위에 있는 컴포넌트를 추출
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        //AudioSource컴포넌트 추출
        _audio = GetComponent<AudioSource>();
        //Shake 스크립트를 추출
        shake = GameObject.Find("CameraRig").GetComponent<Shake>();
	}
	
	// Update is called once per frame
	void Update () {
		//마우스 왼쪽 버튼을 클릭했을때 Fire함수 호출
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
	}

    void Fire()
    {
        //셰이크 효과 호출
        StartCoroutine(shake.ShakeCamera());   //0.3f, 0.4f,0.7f
        //Bullet 프리팹을 동적으로 생성--> 총알공장으로 만든다
        Instantiate(bullet, firePos.position, firePos.rotation);
        //파티클 실행
        cartridge.Play();
        //총구 화염 파티클 실행
        muzzleFlash.Play();
        //사운드발생
        FireSfx();
    }
    void FireSfx()
    {
        //현재 들고 있는 무기의 오디오 클립을 가져옴
        var _sfx = playerSfx.fire[(int)currWeapon];  //**(int)
        //사운드 발생 (오디오클립, 볼륨)
        _audio.PlayOneShot(_sfx, 0.5f);
    }

}
