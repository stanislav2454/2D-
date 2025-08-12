using UnityEngine;

public class GroundDetector : MonoBehaviour
{
}
// using UnityEngine;

//public class GroundDetector : MonoBehaviour
//{
//    private int _groundContactCount;

//    public bool IsGrounded => _groundContactCount > 0;
//    public bool JustLanded { get; private set; }
//    public bool JustLeftGround { get; private set; }
//    private bool _wasGrounded;

//    private void FixedUpdate()
//    {
//        JustLanded = !_wasGrounded && IsGrounded;
//        JustLeftGround = _wasGrounded && !IsGrounded;
//        _wasGrounded = IsGrounded;
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.collider.TryGetComponent<Ground>(out _))
//            _groundContactCount++;
//    }

//    private void OnCollisionExit(Collision collision)
//    {
//        if (collision.collider.TryGetComponent<Ground>(out _))
//            _groundContactCount = Mathf.Max(0, _groundContactCount - 1);
//    }
//}
/*
 // четверг, 7 августа 2025

  //Eсли у вас пол больше чем из одного коллайдера состоит (платформы, стоящие рядом, или ящик),
  //то при переходе с одной платформы на другую ваш текущий детектор земли почти со 100% вероятностью
  //забагуется, тк он сначала войдет в контакт с новой платформой (что воспримет как приземление)
  //и только потом потеряет контакт со старой, что для вашего детектора будет выглядеть как потеря
  //контакта вовсе (прыжок). Следовательно, прыгнуть вы больше не сможете.
  //Так что корректнее не просто полагаться на обнаружение контакта, а вести счет этих контактов
  //(смысл, как в дз про скобки в первом курсе).
*/