using NaughtyAttributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class TaskManager : MonoBehaviour
{
    [SerializeField] GameObject[] fetchableItems;
    [SerializeField, ReadOnly] List<ItemPosition> itemPositions = new List<ItemPosition>();

    public GameObject GenerateFetchableItem()
    {
        GameObject current = Instantiate(fetchableItems[Random.Range(0, fetchableItems.Length)]);

        current.transform.position = itemPositions[Random.Range(0, itemPositions.Count)].transform.position;

        return current;
    }

    [Button]
    async void FindAllItemPositions()
    {
        itemPositions.Clear();

        ItemPosition[] positions = FindObjectsByType<ItemPosition>(FindObjectsSortMode.None);

        for (int i = 0; i < positions.Length; i++)
        {
            itemPositions.Add(positions[i]);

            await Task.Delay(10);
        }
    }
}
