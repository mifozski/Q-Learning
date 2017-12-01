using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using QLearning.MachineLearning.NeuralNetwork;

public class AgentBrains : MonoBehaviour {

	NeuralNetwork nn;

	// Use this for initialization
	void Start () {
		
		nn = new NeuralNetwork(2, new [] { 3 }, 1);

		List<DataSet> dataSet = new List<DataSet>();
		dataSet.Add(new DataSet(new double [] { 1.0f, 1.0f }, new double [] { 0.0f }));
		dataSet.Add(new DataSet(new double [] { 1.0f, 0.0f }, new double [] { 1.0f }));
		dataSet.Add(new DataSet(new double [] { 0.0f, 1.0f }, new double [] { 1.0f }));
		dataSet.Add(new DataSet(new double [] { 0.0f, 0.0f }, new double [] { 0.0f }));
		int epochNum;
		double error = nn.Train(dataSet, 0.01f, out epochNum);

		// Debug.Log("Error: " + error);
		// Debug.Log("Epoch Num: " + epochNum);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}