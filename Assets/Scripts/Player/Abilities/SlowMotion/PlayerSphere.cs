using UnityEngine;

public class PlayerSphere : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private SomeStorage timer;
    [SerializeField] private float chargeSpeed;
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask explosionLayers;

    [SerializeField] private LayerMask contactLayers;
    
    void Start()
    {
        
    }

    private bool _move = true;
    void Update()
    {
        if(_move)
            Move();
        else
        {
            if(!_used)
                Stay();    
        }
    }

    private bool _used = false;
    private void Stay()
    {
        timer.ChangeCurrentValue(chargeSpeed * Time.deltaTime);
        if (timer.CurrentValue == timer.MaxValue)
        {
            Collider[] colliders = new Collider[10];
            Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, colliders, explosionLayers);

            for (int n = 0; n< 10; n++)
            {
                if(colliders[n] != null)
                    colliders[n].GetComponent<ICastSphereTake>().CastSphereTake();
            }

            _used = true;
        }

    }
    
    private void ChangeState()
    {
        _move = false;
    }

    private void Move()
    {
         Vector3 nextPos = transform.forward * moveSpeed;
         transform.Translate(nextPos * moveSpeed * Time.deltaTime);

         RaycastHit hit;
         if (Physics.Raycast(transform.position, transform.forward, out hit, 0.1f, contactLayers,
                 QueryTriggerInteraction.Ignore))
         {
             ChangeState();
         }
    }
}
