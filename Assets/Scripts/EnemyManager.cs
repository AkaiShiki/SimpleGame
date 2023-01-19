using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] float _distanceMax;
    [SerializeField] float _distancePlayer;
    [SerializeField] GameObject _player;

    [SerializeField]  float _vitesse;
    [SerializeField]  float _vitesseTranquille;
    [SerializeField]  float _vitesseRapide;

    [SerializeField]  float _distanceMaxPoint;
    [SerializeField]  float _distanceAttack;

    [SerializeField]  Animator _animator;

    [SerializeField]  FloatVariable _HealthPlayer;
    [SerializeField]  IntVariable _Damage;
    [SerializeField] float _delayDamage;

    //bool _coroutine;
    Vector3 _newPosition;
    bool _coroutineDamage;


    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _distancePlayer = Vector3.Distance(_player.transform.position,gameObject.transform.position);
        _newPosition = CreatePoint();
        _vitesse = _vitesseTranquille;
    }


    void Update()
    {
        _distancePlayer = Vector3.Distance(_player.transform.position,transform.position);

        if(_distancePlayer < _distanceMax){

            MoveToObject(_player.transform.position); // je vais vers le player

            _animator.SetBool("SpeedDown",false); // je joue l'anim rapiement
            _vitesse = _vitesseRapide; // vitesse rapide

            // l'ennemy attack**************
            if(_distancePlayer <_distanceAttack)
            {
                _animator.SetBool("Attack",true);
            }else{
                _animator.SetBool("Attack",false);
            }
            //******************
                     
        }else{

            _animator.SetBool("SpeedDown",true); // anim tranquille
            _vitesse = _vitesseTranquille; // vitesse tranquillou

            float _distancePoint = Vector3.Distance(_newPosition,transform.position);
            if(_distancePoint<1f)
            {
                _newPosition = CreatePoint();

                // while(_newPosition.x>25f || _newPosition.x<-25f || _newPosition.z<-25f || _newPosition.z>25f){
                //     _newPosition = CreatePoint();
                // }
            }

            MoveToObject(_newPosition); // 
        }
    }

    Vector3 CreatePoint(){

        Vector3 _pointPosition = new Vector3(Random.Range(-_distanceMaxPoint,_distanceMaxPoint)+gameObject.transform.position.x,gameObject.transform.position.y,Random.Range(-_distanceMaxPoint,_distanceMaxPoint)+gameObject.transform.position.z);
        return _pointPosition;

    }


    void MoveToObject(Vector3 _movTo){

        float _distanceMoveTo = Vector3.Distance(_movTo,gameObject.transform.position);
        gameObject.transform.LookAt(_movTo);
        Vector3 _vectorMove = _movTo-gameObject.transform.position;             
        gameObject.transform.position += _vectorMove.normalized * _vitesse;

    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Projectile")){
           gameObject.SetActive(false);
        }
 
    }

    
    void OnCollisionStay(Collision other)
    {
        if(other.gameObject.CompareTag("Mur")){
            Debug.Log("on touche mur");
           _newPosition = CreatePoint();  
        }

        if(other.gameObject.CompareTag("Player") && !_coroutineDamage){   
          StartCoroutine(DamageCoroutine());
        }
    }

    
    IEnumerator DamageCoroutine(){
        _coroutineDamage = true;

        _HealthPlayer.Value -= _Damage.Value; 
        yield return new WaitForSeconds(_delayDamage);
       
        _coroutineDamage = false;

    }


}
