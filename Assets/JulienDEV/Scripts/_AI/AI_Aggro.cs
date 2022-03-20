using UnityEngine;

public class AI_Aggro : MonoBehaviour
{
    [SerializeField] private string _targetTag = null;
    [SerializeField] private GameObject[] _enemy = null;
    private AI_Base[] _aiBase = null;

    //[SerializeField] private GameObject _enemy = null;
    //private AI_Base _aiBase = null;

    private void Awake()
    {
        _aiBase = new AI_Base[_enemy.Length];

        for (int i = 0; i < _enemy.Length; i++)
        {
            _aiBase[i] = _enemy[i].GetComponent<AI_Base>();
        }

        //_aiBase = _enemy.GetComponent<AI_Base>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == _targetTag)
        {
            for(int i = 0; i < _aiBase.Length; i++)
            {
                _aiBase[i].AggroPull();
                Debug.Log("Aggro!");
            }

            //_aiBase[i].AggroPull();

            
        }
    }
}