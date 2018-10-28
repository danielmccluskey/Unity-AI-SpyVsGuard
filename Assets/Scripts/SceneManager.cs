using UnityEngine;
using UnityEngine.AI;

public class SceneManager : MonoBehaviour
{
    public Object agentPrefab;
    public int numOfAgents = 1;
    public float spawnRange = 10.0f;

    private void Start()
    {
        //int patientZero = Random.Range(0, numOfAgents);

        //for (int i = 0; i < numOfAgents; ++i)
        //{
        //    Vector2 randomCircle = (Random.insideUnitCircle * spawnRange);
        //    Vector3 randomPoint = Vector3.zero + (Random.insideUnitSphere * spawnRange);
        //    NavMeshHit hit;

        //    if (NavMesh.SamplePosition(randomPoint, out hit, 100.0f, NavMesh.AllAreas))
        //    {
        //        GameObject go = (GameObject)Instantiate(agentPrefab, hit.position, Quaternion.identity);
        //        if (i == patientZero)
        //        {
        //            // This guy is patient zero
        //            //Agent agent = go.GetComponent<Agent>();
        //            // agent.isInfected = true;

        //            // Make patient zero red
        //            Renderer renderer = go.GetComponent<Renderer>();
        //            renderer.material.color = Color.red;
        //        }
        //    }
        //}
    }
}