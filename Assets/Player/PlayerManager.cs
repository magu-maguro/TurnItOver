using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject CPUPrefab;
    [SerializeField] private Vector3[] spawnPoints1 = new Vector3[]
    {
        new Vector3(-10, 0, 0),
    };
    [SerializeField] private Vector3[] spawnPoints2 = new Vector3[]
    {
        new Vector3(-10, 1.5f, 0),
        new Vector3(-10, -1.5f, 0),
    };
    [SerializeField] private Vector3[] spawnPoints3 = new Vector3[]
    {
        new Vector3(-10, -3, 0),
        new Vector3(-10, 0, 0),
        new Vector3(-10, 3, 0),
    };
    private List<Vector3[]> spawnPointsList;

    private GameObject player;
    private List<GameObject> cpuList = new List<GameObject>();

    void Start()
    {
        spawnPointsList = new List<Vector3[]>() { spawnPoints1, spawnPoints2, spawnPoints3 };
    }

    public void SpawnPlayer(int cpuNum, float cpuAccuracy)
    {
        // プレイヤーの生成
        player = Instantiate(playerPrefab, new Vector3(-10, 0, 0), Quaternion.identity);
        player.GetComponent<PlayerController>().DisallowInput();

        // CPUの生成
        for (int i = 0; i < cpuNum; i++)
        {
            GameObject cpu = Instantiate(CPUPrefab, spawnPointsList[cpuNum-1][i], Quaternion.identity);
            cpu.GetComponent<CpuController>().DisallowInput();
            cpu.GetComponent<CpuController>().accuracy = cpuAccuracy;
            cpuList.Add(cpu);
        }
    }

    public void AllowPlayerInput()
    {
        player.GetComponent<PlayerController>().AllowInput();
        foreach (var cpu in cpuList)
        {
            cpu.GetComponent<CpuController>().AllowInput();
        }
    }

    public void DisallowPlayerInput()
    {
        player.GetComponent<PlayerController>().DisallowInput();
        foreach (var cpu in cpuList)
        {
            cpu.GetComponent<CpuController>().DisallowInput();
        }
    }
}
