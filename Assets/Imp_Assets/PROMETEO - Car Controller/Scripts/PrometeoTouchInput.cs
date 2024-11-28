using UnityEngine;
using UnityEngine.Serialization;
namespace Imp_Assets.PROMETEO___Car_Controller.Scripts
{
  public class PrometeoTouchInput : MonoBehaviour
  {

    [FormerlySerializedAs("changeScaleOnPressed")] public bool ChangeScaleOnPressed = false;
    [FormerlySerializedAs("buttonPressed")] [HideInInspector]
    public bool ButtonPressed = false;
    RectTransform _rectTransform;
    Vector3 _initialScale;
    float _scaleDownMultiplier = 0.85f;

    void Start(){
      _rectTransform = GetComponent<RectTransform>();
      _initialScale = _rectTransform.localScale;
    }

    public void ButtonDown(){
      ButtonPressed = true;
      if(ChangeScaleOnPressed){
        _rectTransform.localScale = _initialScale * _scaleDownMultiplier;
      }
    }

    public void ButtonUp(){
      ButtonPressed = false;
      if(ChangeScaleOnPressed){
        _rectTransform.localScale = _initialScale;
      }
    }

  }
}
