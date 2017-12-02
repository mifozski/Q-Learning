using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using QLearning.MachineLearning.NeuralNetwork;

public class AgentBrains : MonoBehaviour {

	NeuralNetwork nn;
	AgentController controller;

	// Use this for initialization
	void Start () {
		
		nn = new NeuralNetwork(1, new [] { 11 }, 5);

		controller = GetComponent<AgentController>();

		List<DataSet> dataSet = new List<DataSet>();
		dataSet.Add(new DataSet(new double [] { 1.0f, 1.0f }, new double [] { 0.0f }));
		dataSet.Add(new DataSet(new double [] { 1.0f, 0.0f }, new double [] { 1.0f }));
		dataSet.Add(new DataSet(new double [] { 0.0f, 1.0f }, new double [] { 1.0f }));
		dataSet.Add(new DataSet(new double [] { 0.0f, 0.0f }, new double [] { 0.0f }));
		int epochNum;
		double error = nn.Train(dataSet, 0.01f, out epochNum);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (controller.GetInternalState().energy == 0.0f)
			SceneManager.Get().KillAgent(gameObject);
	}

	void FixedUpdate()
	{
		var ouputs = nn.Compute(new double [] { controller.GetVisibleEnemiesNum() } );

		// 	Outputs (5):
		// 0. Move "Left"
		// 1. Move "Right"
		// 2. Move "Up"
		// 3. Move "Down"
		// 4. Back off

		if (ouputs[4] > 0.5)
			controller.MoveBackwards(true);
		else
		{
			float left = Convert.ToSingle(ouputs[0]);
			float right = Convert.ToSingle(ouputs[1]);
			float up = Convert.ToSingle(ouputs[2]);
			float down = Convert.ToSingle(ouputs[3]);
			controller.Move(new Vector2(right - left, up - down));
		}
	}
}