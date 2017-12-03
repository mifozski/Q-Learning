using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using QLearning.MachineLearning.NeuralNetwork;

public class AgentBrains : MonoBehaviour {

	NeuralNetwork nn;
	AgentController controller;

	[SerializeField]
	bool save = false;

	int inputNumber = 4;
	int outputNumber = 5;

	double [] lastInput;
	double [] lastOutput;

	float totalReward;

	// Use this for initialization
	void Start () {
		
		nn = new NeuralNetwork(inputNumber, new [] { 20 }, outputNumber);

		lastInput = new double [inputNumber];
		lastOutput = new double [outputNumber];

		controller = GetComponent<AgentController>();

		// List<DataSet> dataSet = new List<DataSet>();
		// dataSet.Add(new DataSet(new double [] { 1.0f, 1.0f }, new double [] { 0.0f }));
		// dataSet.Add(new DataSet(new double [] { 1.0f, 0.0f }, new double [] { 1.0f }));
		// dataSet.Add(new DataSet(new double [] { 0.0f, 1.0f }, new double [] { 1.0f }));
		// dataSet.Add(new DataSet(new double [] { 0.0f, 0.0f }, new double [] { 0.0f }));
		// int epochNum;
		// double error = nn.Train(dataSet, 0.01f, out epochNum);
	}
	
	// Update is called once per frame
	void Update ()
	{
		// 'Kill' agent if the energy is depleted
		if (controller.GetInternalState().energy == 0.0f)
		{
			// 
			totalReward -= -1.0f;

			
			
			SceneManager.Get().KillAgent(gameObject);

			totalReward = 0.0f;
		}

		// Evaluate output values
		lastInput = new double [] {
			controller.GetNormalizedEnergyLevel(),
			controller.GetHeardNoiseLevel(),
			controller.GetVisibleEnemiesNum(),
			controller.GetNormalizedAngleToNearestAgent()
		};

		var ouputs = nn.Compute(lastInput);

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

		lastOutput = ouputs;
	}

	public float [] GetLastInputs()
	{
		float [] floatInputs = Array.ConvertAll(lastInput, x => (float)x);
		return floatInputs;
	}

	public float [] GetLastOuputs()
	{
		float [] floatOutputs = Array.ConvertAll(lastOutput, x => (float)x);
		return floatOutputs;
	}

	void OnGUI()
	{
		
	}
}