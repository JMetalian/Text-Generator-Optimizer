//Adaptive Optimization Implementation Project 2
//Text Generator Optimizer
//Written on C# using Unity Engine
//Used Algorithm for optimization: Genetic Algorithm
//İrem Ayvaz-Abdulwahab Hajar-Can Kozan-İbrahim Krasniqi


using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class OperationArea : MonoBehaviour
{
	[Header("Genetic Algorithm Operation Area")]
	[SerializeField] string targetString = "";
	[SerializeField] string availableChars = "0987654321abcçdefgğhijklmnopqrsştuüvwxyzABCÇDEFGHIİJKLMNOPQRSŞTUÜVWXYZ,<>'^#+%.|!#$%&/()=?;,~ ";
	[SerializeField] int popSize;
	[SerializeField] float mutationRate;
	[SerializeField] int eliteChoosing;

	[Header("Unity Elemets")]
	[SerializeField] Text textObjectField;
	[SerializeField] Text targetText;
	[SerializeField] Text bestText;
	[SerializeField] Text bestFitnessText;
	[SerializeField] Text numGenerationsText;
	[SerializeField] Transform populationTextParent;
	
	int numCharsPerText = 20000;

	private GeneticAlgorithm<char> nChar;
	private System.Random random;

	void Start()
	{
		targetText.text = targetString;

		if (string.IsNullOrEmpty(targetString))
		{
			this.enabled = false;
		}

		random = new System.Random();
		nChar = new GeneticAlgorithm<char>(popSize, targetString.Length, random, GetRandomCharacter, FitnessFunction, eliteChoosing, mutationRate);
	}

	void Update()
	{
		nChar.NewGeneration();

		UpdateText(nChar.BestGenes, nChar.BestFitness, nChar.Generation, nChar.Population.Count, (j) => nChar.Population[j].Genes);

		if (nChar.BestFitness == 1)
		{
			this.enabled = false;
		}
	}

	private char GetRandomCharacter()
	{
		int i = random.Next(availableChars.Length);
		return availableChars[i];
	}

	private float FitnessFunction(int index)
	{
		float score = 0;
		DNA<char> dna = nChar.Population[index];

		for (int i = 0; i < dna.Genes.Length; i++)
		{
			if (dna.Genes[i] == targetString[i])
			{
				score += 1;
			}
		}

		score /= targetString.Length;

		score = (Mathf.Pow(2, score) - 1) / (2 - 1);

		return score;
	}
	private int numCharsPerTextObj;
	private List<Text> textList = new List<Text>();

	void Awake()
	{
		numCharsPerTextObj = numCharsPerText / availableChars.Length;
		if (numCharsPerTextObj > popSize) numCharsPerTextObj = popSize;

		int numTextObjects = Mathf.CeilToInt((float)popSize / numCharsPerTextObj);

		for (int i = 0; i < numTextObjects; i++)
		{
			textList.Add(Instantiate(textObjectField, populationTextParent));
		}
	}

	private void UpdateText(char[] bestGenes, float bestFitness, int generation, int popSize, Func<int, char[]> getGenes)
	{
		bestText.text = CharArrayToString(bestGenes);
		bestFitnessText.text = bestFitness.ToString();

		numGenerationsText.text = generation.ToString();

		for (int i = 0; i < textList.Count; i++)
		{
			var nVar = new StringBuilder();
			int endIndex = i == textList.Count - 1 ? popSize : (i + 1) * numCharsPerTextObj;
			for (int j = i * numCharsPerTextObj; j < endIndex; j++)
			{
				foreach (var c in getGenes(j))
				{
					nVar.Append(c);
				}
				if (j < endIndex - 1) nVar.AppendLine();
			}

			textList[i].text = nVar.ToString();
		}
	}

	private string CharArrayToString(char[] charArray)
	{
		var nVar = new StringBuilder();
		foreach (var c in charArray)
		{
			nVar.Append(c);
		}

		return nVar.ToString();
	}
}
