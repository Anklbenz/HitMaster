using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Sword : MonoBehaviour
{
    [SerializeField] private float flyingSpeed,hitForce;
    [SerializeField] private int damage;
    [SerializeField] private ParticleSystem trailParticles;

    private Transform _poolTransform;
    private BoxCollider _collider;
    private Rigidbody _rigidBody;
    private Vector3 _direction;

    private void Awake(){
        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _poolTransform = transform.parent;
    }

    public void Initialize(Vector3 pos, Vector3 direction){
        enabled = true;
        trailParticles.Play();

        _direction = (direction - pos).normalized;
        _collider.enabled = true;
        _rigidBody.isKinematic = false;
        transform.parent = _poolTransform;

        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(_direction);
    }

    private void FixedUpdate(){
        _rigidBody.MovePosition(transform.position + flyingSpeed * _direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other){
        Hit(other);
        Disable();
    }

    private void Hit(Collider other){
        var hitPoint = other.ClosestPoint(transform.position);

        StuckInTarget(hitPoint, other.transform);
        TargetDamage(hitPoint, other);
    }

    private void StuckInTarget(Vector3 hitPoint, Transform parent){
        _rigidBody.isKinematic = true;
        _collider.enabled = false;

        transform.SetParent(parent);
        transform.InverseTransformPoint(hitPoint);
    }

    private void TargetDamage(Vector3 hitPoint, Collider other){
        var enemy = other.GetComponentInParent<Enemy>();
        var hitRigidbody = other.attachedRigidbody;
        if (!enemy || !hitRigidbody) return;

        enemy.TakeDamage(other, damage);
        
        hitRigidbody.AddForceAtPosition(_direction * hitForce, hitPoint, ForceMode.Impulse);
    }

    private void Disable(){
        trailParticles.Stop();
        this.enabled = false;
    }
}