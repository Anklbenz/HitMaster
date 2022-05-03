using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RagDollSystem
{
    private readonly Animator _animator;
    private readonly Transform _mainTransform, _hipsTransform, _legTransform;
    private readonly List<BoneData> _boneDataList = new List<BoneData>();
    private Vector3 _newHipsPosition;
    private float _savedHipsPositionY, _savedTransformPositionY;
    private bool _gettingUp;

    public RagDollSystem(Rigidbody[] ragDollBodies, Transform mainTransform, Animator animator){
        foreach (var rigidbody in ragDollBodies)
            _boneDataList.Add(new BoneData(rigidbody));

        _animator = animator;
        _mainTransform = mainTransform;
        _hipsTransform = _animator.GetBoneTransform(HumanBodyBones.Hips);
        _legTransform = _animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
    }

    public void Upd(){
        if (!_gettingUp) return;
        if (!RestoreBonePositions()) return;
        _gettingUp = false;
        _animator.enabled = true;
    }

    public void RagDollOn(){
        _animator.enabled = false;
        SaveBonePositions();
        SettingsDollOn(true);
    }

    public async void GetUp(int delayMilliseconds){
        await Task.Delay(delayMilliseconds);

        SettingsDollOn(false);
        var currentHipsPosition = _hipsTransform.position;
        _mainTransform.position = new Vector3(currentHipsPosition.x, _savedTransformPositionY, currentHipsPosition.z);
        _newHipsPosition =  _hipsTransform.localPosition;
        _newHipsPosition.y = _savedHipsPositionY;
        _gettingUp = true;
    }

    private void SaveBonePositions(){
        _savedHipsPositionY = _hipsTransform.localPosition.y;//
        _savedTransformPositionY = _mainTransform.position.y;

        foreach (var bone in _boneDataList)
            bone.SavedRotation = bone.Transform.rotation;
    }

    private bool RestoreBonePositions(){
        foreach (var bone in _boneDataList)
            bone.Transform.rotation = Quaternion.Slerp(bone.Transform.rotation, bone.SavedRotation,  Time.deltaTime);
        _hipsTransform.localPosition = Vector3.Slerp(_hipsTransform.localPosition, _newHipsPosition,  Time.deltaTime);
        
        return (_hipsTransform.localPosition - _newHipsPosition).magnitude < 0.03;
    }

    public void SettingsDollOn(bool state){
        foreach (var bone in _boneDataList){
            if (bone.Transform == _mainTransform){
                bone.Rigidbody.isKinematic = state;
                bone.Collider.isTrigger = state;
                continue;
            }

            bone.Rigidbody.isKinematic = !state;
            bone.Collider.isTrigger = !state;
        }
    }
}

public class BoneData
{
    public Transform Transform{ get; }
    public Rigidbody Rigidbody{ get; }
    public Collider Collider{ get; }
    public Quaternion SavedRotation{ get; set; }

    public BoneData(Rigidbody body){
        Rigidbody = body;
        Transform = body.transform;
        Collider = Transform.GetComponent<Collider>();
    }
}