using UnityEngine;
using TinkerWorX.AccidentalNoiseLibrary;

public class Generator : MonoBehaviour {

	[SerializeField]
	int Width = 256;

	[SerializeField]
	int Height = 256;

	[SerializeField]
	int TerrainOctaves = 6;

	[SerializeField]
	double TerrainFrequency = 1.56;

	ImplicitFractal HeightMap;

	MapData HeightData;

	Tile[,] Tiles;

	MeshRenderer HeightMapRenderer;

	// Use this for initialization
	void Start () 
	{
		//create Mesh with renderer data
		HeightMapRenderer = transform.Find ("HeightTexture").GetComponent();

		//initialize generator;
		Initialize();

		//create map of heights
		GetData (HeightMap, ref HeightData);

		//Create ends object witch bases on our data
		LoadTiles();

		//Rendering our shit 
		HeightMapRenderer.materials[0].mainTexture = TextureGenerator.GetTexture(Width, Height, Tiles);
	}

	private void Initialize() 
	{
		HeightMap = new ImplicitFractal (FractalType.Multi, BasisType.Simplex, InterpolationType.Quintic, TerrainOctaves, TerrainFrequency, UnityEngine.Random.Range(0, int.MaxValue));

	}

	private void GetData (ImplicitModuleBase module, ref MapData mapData)
	{
		mapData = new MapData (Width, Height);

		for (var x = 0; x < Width; x++)
		{
			for (var y = 0; y < Height;  y++)
			{
				float x1 = x/(float)Width;
				float y1 = y/(float)Height;

				float value = (float) HeightMap.Get (x1,y1);

				if (value > mapData.Max) mapData.Max = value;
				if (value < mapData.Min) mapData.Min = value;

				mapData[x,y] = value;
			}
		}
	}

	private void LoadTiles()
	{
		Tiles = new Tile[Width,Height];

		for (var x = 0; x < Width; x++)
		{
			for (var y = 0; y < Height;  y++)
			{
				Tile t = new Tile();
				t.X = x;
				t.Y = y;

				float value = HeightData.Data[x,y];

				//normalization
				value = (value = HeightData.Min)/(HeightData.Max - HeightData.Min);

				t.HeightValue = value;

				Tiles[x,y] = t;
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}

