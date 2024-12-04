using UnityEngine;
namespace _Scripts
{
    public class MarkerManager : MonoBehaviour
    {
        [SerializeField] GameObject MarkerPrefab;
        GameObject currentMarker;

        public void AddMarker(Transform t)
        {
            if (currentMarker is not null) Destroy(currentMarker);

            currentMarker = Instantiate(MarkerPrefab, t.transform.position + Vector3.up * 10, t.transform.rotation);
            currentMarker.transform.localScale = Vector3.one*10;
        }
        public void RemoveMarker()
        {
            if (currentMarker is not null) Destroy(currentMarker);
        }
    }
}
