//Adaptive Optimization Implementation Project 2
//Text Generator Optimizer
//Written on C# using Unity Engine
//Used Algorithm for optimization: Genetic Algorithm
//İrem Ayvaz-Abdulwahab Hajar-Can Kozan-İbrahim Krasniqi

using System;
using System.Collections.Generic;

public class GeneticAlgorithm<T>
{
	public List<DNA<T>> Population { 
	get;
	 private set; 
	 }
	public int Generation {
	 get;
	 private set;
	 }
	public float BestFitness {
	 get; 
	private set;
	 }
	public T[] BestGenes {
	 get;
	  private set; 
	  }
	private Random random;

	public int Elitism;
	public float MutationRate;
	
	private List<DNA<T>> newPopulation;
	private float fitnessSum;
	private int sizeofDNA;
	private Func<T> getRandomGene;
	private Func<int, float> fitnessFunction;

	public GeneticAlgorithm(int populationSize, int sizeofDNA, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction,
		int elitism, float mutationRate = 0.01f) //In default mutation rate is 0.01
	{
		Generation = 1;
		Elitism = elitism;
		MutationRate = mutationRate;
		Population = new List<DNA<T>>(populationSize);
		newPopulation = new List<DNA<T>>(populationSize);
		this.random = random;
		this.sizeofDNA = sizeofDNA;
		this.getRandomGene = getRandomGene;
		this.fitnessFunction = fitnessFunction;

		BestGenes = new T[sizeofDNA];

		for (int i = 0; i < populationSize; i++)
		{
			Population.Add(new DNA<T>(sizeofDNA, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
		}
	}

	public void NewGeneration(int numNewDNA = 0, bool crossoverNewDNA = false)
	{
		int finalCount = Population.Count + numNewDNA;

		if (finalCount <= 0) {
			return;
		}

		if (Population.Count > 0) {
			CalculateFitness();
			Population.Sort(CompareDNA);
		}
		newPopulation.Clear();

		for (int i = 0; i < Population.Count; i++)
		{
			if (i < Elitism && i < Population.Count)
			{
				newPopulation.Add(Population[i]);
			}
			else if (i < Population.Count || crossoverNewDNA)
			{
				DNA<T> parent1 = Select_Parent();
				DNA<T> parent2 = Select_Parent();

				DNA<T> child = parent1.Crossing_Over(parent2);

				child.Mutation_Function(MutationRate);

				newPopulation.Add(child);
			}
			else
			{
				newPopulation.Add(new DNA<T>(sizeofDNA, random, getRandomGene, fitnessFunction, shouldInitGenes: true));
			}
		}

		List<DNA<T>> tmpList = Population;
		Population = newPopulation;
		newPopulation = tmpList;

		Generation++;
	}
	
	private int CompareDNA(DNA<T> a, DNA<T> b)
	{
		if (a.Fitness > b.Fitness) {
			return -1;
		} else if (a.Fitness < b.Fitness) {
			return 1;
		} else {
			return 0;
		}
	}

	private void CalculateFitness()
	{
		fitnessSum = 0;
		DNA<T> best = Population[0];

		for (int i = 0; i < Population.Count; i++)
		{
			fitnessSum += Population[i].CalculateFitness(i);

			if (Population[i].Fitness > best.Fitness)
			{
				best = Population[i];
			}
		}

		BestFitness = best.Fitness;
		best.Genes.CopyTo(BestGenes, 0);
	}

	private DNA<T> Select_Parent()
	{
		double randomNumber = random.NextDouble() * fitnessSum;

		for (int i = 0; i < Population.Count; i++)
		{
			if (randomNumber < Population[i].Fitness)
			{
				return Population[i];
			}

			randomNumber -= Population[i].Fitness;
		}

		return null;
	}
}
