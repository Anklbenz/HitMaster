using UnityEngine;

[ExecuteAlways]
[SelectionBase]
public class RepeatObject : MonoBehaviour
{
 
    [SerializeField] private Enemy Prefab;
    [Range(0, 5)] [SerializeField] private int Count;
    [Space]
    [SerializeField] private Vector3 offset;

    private void Start(){
        if (Application.IsPlaying(this))
            Destroy(this);
    }

    private void Update(){
        if (Prefab)
            SetChildCount(Prefab, Count);
    }

    private void SetChildCount(Enemy prefab, int count){
        var createdElementsCount = transform.childCount;

        if (createdElementsCount < count){
            for (var i = createdElementsCount; i < count; i++){
                var obj = Instantiate(prefab, transform);
                obj.transform.Translate(offset * i);
            }
        }
        else if (createdElementsCount > count){
            for (var i = createdElementsCount - 1; i >= count; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}
