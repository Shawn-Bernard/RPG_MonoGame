using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace RPG_MonoGame_ShawnBernard
{
    public class Map : Entity
    {

        TurnBasedSystem turnBasedSystem;

        System.Random rng = new();

        private string path = $"{Environment.CurrentDirectory}/../../../Maps/";

        public Dictionary<Vector2, int> tileMap;

        int rngX;

        int rngY;

        private List<string> Maps = new List<string>();

        Texture2D wallTexture, groundTexture, exitTexture;

        public Vector2 startPosition;
        public Map()
        {
            AddPreMadeMaps();
        }

        public override void OnAddedToScene()
        {
            Debug.Log($"{Environment.CurrentDirectory}/../../../Maps/");
            turnBasedSystem = Scene.FindComponentOfType<TurnBasedSystem>();
            
            if (turnBasedSystem != null )
            {
                Debug.Log("im here");
            }
            wallTexture = Scene.Content.Load<Texture2D>("Wall");
            groundTexture = Scene.Content.Load<Texture2D>("Ground");
            exitTexture = Scene.Content.Load<Texture2D>("Exit");
            MapStyle();

            
        }

        public void MapStyle()
        {
            int mapRPNG = rng.Next(0, 2);
            //Picking which map style to load
            switch (mapRPNG)
            {
                case 0:
                    tileMap = InitializeMap();
                    break;
                case 1:
                    tileMap = TextMap(path + PickRandomMap());
                    break;
            }
            loadMap();

        }

        private Dictionary<Vector2, int> InitializeMap()
        {
            Dictionary<Vector2, int> MapGen = new Dictionary<Vector2, int>();
            rngX = rng.Next(15, 30);
            rngY = rng.Next(15, 30);

            //Step 1 initializing map
            for (int x = 0; x < rngX; x++)
            {
                for (int y = 0; y < rngY; y++)
                {
                    //Adding my base map with only floors
                    MapGen.Add(new Vector2(x, y), 1);
                }
            }

            //Step 2: placing walls
            for (int x = 0; x < rngX; x++)
            {
                for (int y = 0; y < rngY; y++)
                {
                    //If statement that checks arounf the borders
                    if (x == 0 || y == 0 || x == rngX - 1 || y == rngY - 1)
                    {
                        MapGen[new Vector2(x, y)] = 0;
                    }
                }
            }

            //Step 3: Place clusters
            //Making a amount of clusters equal to the max map count
            int clusterCount = MapGen.Count;
            for (int i = 0; i < clusterCount; i++)
            {
                //Picking my start x & y for the to start clusters
                int clusterX = rng.Next(1, rngX - 2);
                int clusterY = rng.Next(1, rngY - 2);

                //Picking a random width & height for my clusters
                int clusterWidth = rng.Next(2, 4);
                int clusterHeight = rng.Next(2, 4);

                //Making a bool to check if i can place
                bool canPlace = true;

                //Checking a negative space so we can look around the cluster to see if we can place 
                //The reason being check back 1 space (-1) then forward space (width + 1)
                for (int x = -1; x < clusterWidth + 1; x++)
                {
                    for (int y = -1; y < clusterHeight + 1; y++)
                    {
                        Vector2 checkPosition = new Vector2(clusterX + x, clusterY + y);
                        //If my map has the key position & the value of 0 can place is false
                        if (MapGen.ContainsKey(checkPosition) && MapGen[checkPosition] == 0)
                        {
                            canPlace = false;
                        }
                    }
                }
                //If we can place doing a for loop with the cluster width / height
                if (canPlace)
                {
                    for (int x = 0; x < clusterWidth; x++)
                    {
                        for (int y = 0; y < clusterHeight; y++)
                        {
                            Vector2 ClusterPosition = new Vector2(clusterX + x, clusterY + y);

                            MapGen[ClusterPosition] = 0;
                        }
                    }
                }
            }
            bool playerPlaced = false;
            while (!playerPlaced)
            {
                int playerX = rng.Next(1, rngX - 2);
                int playerY = rng.Next(1, rngY - 2);
                Vector2 playerPosition = new Vector2(playerX, playerY);


                if (MapGen.ContainsKey(playerPosition) && MapGen[playerPosition] == 1)
                {
                    MapGen[playerPosition] = 3;
                    playerPlaced = true;
                }
            }




            //Step 5: placing a random amount of door in a position
            int doorCount = rng.Next(1, 2);
            for (int i = 0; i < doorCount; i++)
            {
                // Picking a random start X & Y for the door placement
                int doorX = rng.Next(1, rngX - 2);
                int doorY = rng.Next(1, rngY - 2);

                Vector2 checkPosition = new Vector2(doorX, doorY);

                if (MapGen.ContainsKey(checkPosition) && MapGen[checkPosition] == 1)
                {
                    MapGen[checkPosition] = 2;
                }
            }
            return MapGen;
        }


        private Vector2 PlaceObjects(Dictionary<Vector2, int> Map, Vector2 entityPosition)
        {
            Vector2 checkPosition = new Vector2(rng.Next(1, rngX - 1), rng.Next(1, rngY - 1));
            //Step 4: Placing my player 
            if (Map.ContainsKey(checkPosition) && Map[checkPosition] == 1)
            {
                Map[checkPosition] = 3;//Need to fix this untill Should be equal to 4 for enemy
                return checkPosition;
            }
            return checkPosition = PlaceObjects(Map, entityPosition);
        }


        // Adding my list of strings for my map
        private void AddPreMadeMaps()
        {
            Maps.Add("Level_1.txt");
            Maps.Add("Level_2.txt");
            Maps.Add("Level_3.txt");
        }

        public int checkTile(Vector2 checkedPosition)
        {
            // Returning an int based on the checked position
            if (tileMap.ContainsKey(checkedPosition))
            {
                return tileMap[checkedPosition];
            }
            else
            {
                return 0;
            }

        }

        //Picks a random map from the list of maps 
        private string PickRandomMap()
        {
            rng = new System.Random();
            int index = rng.Next(Maps.Count);
            return Maps[index];
        }
        private Dictionary<Vector2, int> TextMap(string filepath)
        {
            Dictionary<Vector2, int> result = new Dictionary<Vector2, int>();
            //This will read my map text file
            StreamReader reader = new StreamReader(filepath);
            int y = 0;
            string line;

            //This will give line the value untill the reader is done reading the text file
            while ((line = reader.ReadLine()) != null)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    Vector2 TilePosition = new Vector2(x, y);
                    char tile = line[x];
                    switch (tile)
                    {

                        case '#':
                            //Results will store a Vector2 with a value (Example vector2(0,0) with the value of 0
                            result[TilePosition] = 0;//Walls
                            break;
                        case '-':
                            //This would be the floor
                            result[TilePosition] = 1;//Floor
                            break;
                        case '*':
                            //This would be the player if I got it working
                            result[TilePosition] = 2;//Exit
                            break;
                        case '@':
                            //Player spawn
                            result[TilePosition] = 3;//Player
                            break;
                        case '!':
                            //Enemy spawn
                            result[TilePosition] = 4;//Enemy
                            break;
                    }
                }
                y++;
            }
            return result;
        }
        public void loadMap()
        {
            //The result is the return from Text Map
            foreach (var Result in tileMap)
            {
                Vector2 Position = new Vector2(Result.Key.X * 16, Result.Key.Y * 16);
                
                switch (Result.Value)
                {
                    case 0:
                        addTile(wallTexture, Position);
                        //spriteRenderer.SetTexture(wallTexture).SetOrigin(Position);
                        break;
                    case 1:
                        addTile(groundTexture, Position);
                        //spriteRenderer.SetTexture(groundTexture).SetOrigin(Position);
                        break;
                    case 2:
                        //spriteRenderer.SetTexture(exitTexture).SetOrigin(Position);
                        break;
                    case 3:
                        addTile(groundTexture, Position);
                        Debug.Log("position");
                        Debug.Log(Position);

                        Debug.Log("Result X");
                        Debug.Log(Result.Key.X * 16);
                        Debug.Log("Result Y");
                        Debug.Log(Result.Key.Y * 16);
                        //Actor player = new Actor(new Vector2(Result.Key.X,Result.Key.Y));
                        //turnBasedSystem.AddActor(player);

                        //Debug.Log("player was added in map");
                        //spriteRenderer.SetTexture(groundTexture).SetOrigin(Position);
                        break;
                    case 4:
                        //spriteRenderer.SetTexture(groundTexture).SetOrigin(Position);
                        break;
                }
            }
            foreach (var actor in turnBasedSystem.Actors)
            {
                Scene.AddEntity(actor);
            }
        }
        public void addTile(Texture2D texture, Vector2 position)
        {
            SpriteRenderer tileRenderer = new SpriteRenderer(texture);
            tileRenderer.SetOrigin(position);
            //Debug.Log(tileRenderer.Sprite);
            //Debug.Log(Scale);
            AddComponent(tileRenderer);
        }

        public void removeTile(Texture2D texture, Vector2 position)
        {
            SpriteRenderer tileRenderer = new SpriteRenderer(texture);
            tileRenderer.SetOrigin(position);
            //Debug.Log(tileRenderer.Sprite);
            //Debug.Log(Scale);
            RemoveComponent(tileRenderer);
        }
    }
}
