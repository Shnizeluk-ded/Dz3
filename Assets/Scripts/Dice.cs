using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    //настройки броска
    private const float ForceMin = 3f;     
    private const float ForceMax = 7f;      
    private const float UpForceMin = 4f;    
    private const float UpForceMax = 7f;    
    private const float TorqueMin = 5f;      
    private const float TorqueMax = 15f;    
    private const float YawSpread = 90f;   

    [HideInInspector]public bool isRolling; //кубик ещё летит или катится. Прячем в инспекторе чтоб не мешался

    private Rigidbody body;
    private Vector3 direction;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void Roll()
    {
        isRolling = true;

        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;

        direction = Quaternion.Euler(0f, Random.Range(-YawSpread, YawSpread), 0f) * Vector3.forward;

        body.AddForce(direction * Random.Range(ForceMin, ForceMax), ForceMode.Impulse);
        body.AddForce(Vector3.up * Random.Range(UpForceMin, UpForceMax), ForceMode.Impulse);
        body.AddTorque(Random.onUnitSphere * Random.Range(TorqueMin, TorqueMax), ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (isRolling && body.IsSleeping())
        {
            isRolling = false;
        }
    }

    //какая грань смотрит вверх
    public int GetUpwardValue()
    {
        if (transform.up.y > 0.5f) return 1;
        if (-transform.up.y > 0.5f) return 6;
        if (transform.forward.y > 0.5f) return 2;
        if (-transform.forward.y > 0.5f) return 5;
        if (transform.right.y > 0.5f) return 3;
        return 4;
    }
}