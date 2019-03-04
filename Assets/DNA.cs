//Adaptive Optimization Implementation Project 2
//Text Generator Optimizer
//Written on C# using Unity Engine
//Used Algorithm for optimization: Genetic Algorithm
//İrem Ayvaz-Abdulwahab Hajar-Can Kozan-İbrahim Krasniqi


using System;
public class DNA<T>
{
	private Random random;
	public T[] Genes {
		
		 get;
		 private set; 
		 }
	public float Fitness {
	 get;
	 private set;
	  }

	private Func<T> getRandomGene;
	private Func<int, float> fitnessFunction;

	public DNA(int size, Random random, Func<T> getRandomGene, Func<int, float> fitnessFunction, bool shouldInitGenes = true)
	{
		Genes = new T[size];
		this.random = random;
		this.getRandomGene = getRandomGene;
		this.fitnessFunction = fitnessFunction;

		if (shouldInitGenes)
		{
			for (int i = 0; i < Genes.Length; i++)
			{
				Genes[i] = getRandomGene();
			}
		}
	}

	public float CalculateFitness(int index)
	{
		Fitness = fitnessFunction(index);
		return Fitness;
	}

	public DNA<T> Crossing_Over(DNA<T> otherParent)
	{
		DNA<T> child = new DNA<T>(Genes.Length, random, getRandomGene, fitnessFunction, shouldInitGenes: false);

		for (int i = 0; i < Genes.Length; i++)
		{
			child.Genes[i] = random.NextDouble() < 0.5 ? Genes[i] : otherParent.Genes[i];
		}

		return child;
	}

	public void Mutation_Function(float mutationRate)
	{
		for (int i = 0; i < Genes.Length; i++)
		{
			if (random.NextDouble() < mutationRate)
			{
				Genes[i] = getRandomGene();
			}
		}
	}
}