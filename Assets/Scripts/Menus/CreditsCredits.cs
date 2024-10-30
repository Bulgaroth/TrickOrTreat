using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreditsCredits : MonoBehaviour
{
    [SerializeField] private int nbrCandies = 10;

    [SerializeField] private Transform posA, posB;

    [SerializeField] private List<GameObject> listNamesPrefab = new List<GameObject>();
    [SerializeField] private List<GameObject> listCandies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCandiesCoroutine());
    }

    IEnumerator SpawnCandiesCoroutine()
    {
        for(int i=0; i<nbrCandies; i++)
        {
            yield return new WaitForSeconds(.5f);

            SpawnCandies(ChooseCandy());
        }
    }

    private GameObject ChooseCandy()
    {
        if(listNamesPrefab.Count != 0 && Random.Range(0,2) == 0)
        {
            int randObj = Random.Range(0, listNamesPrefab.Count);
            GameObject obj = listNamesPrefab[randObj];
            listNamesPrefab.Remove(obj);
            return obj;
        }
        int randObj2 = Random.Range(0, listCandies.Count);
        return listCandies[randObj2];
    }

    private void SpawnCandies(GameObject obj)
    {
        float randX = Random.Range(posA.position.x, posB.position.x);
        float randRot = Random.Range(0, 360);
        Vector3 pos = new Vector3(randX, posA.position.y, 0);
        Instantiate(obj, pos, Quaternion.Euler(0,0, randRot));
    }

}
