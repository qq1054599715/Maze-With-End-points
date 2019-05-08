using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZycSolute
{
    [System.Serializable]
    public struct ExpectPosition
    {
        public int x, y;

        public ExpectPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
    }

    public class Cell
    {
        public bool left, right, up, down,visited;
        public int x, y;

        public Cell(int x,int y)
        {
            this.x = x;
            this.y = y;
        
            Reset();
        }

        public void Reset()
        {
            this.left = false;
            this.right = false;
            this.up = false;
            this.down = false;
            this.visited = false;
        }
        
    }

    public class SoluteMaze
    {
        private List<Cell> cells;
        private int width, height;
        
        public SoluteMaze(int width ,int height)
        {
            this.width = width;
            this.height = height;
            cells = new List<Cell>();
            
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Cell cell = new Cell(j,i);
                    cells.Add(cell);
                }
            }
            
        }

        
        private List<Cell> stepStack;
        private Cell stepCurrent;
        private Cell startCell;

        private List<Cell> endCells;
        
        public void BeginSearchMaze(ExpectPosition startPostion,List<ExpectPosition> endPostions)
        {
            foreach (Cell cell in cells)
            {
                cell.Reset();
            }
			startCell = cells[startPostion.y * width + startPostion.x];
			endCells = new List<Cell>();
            foreach (var endPostion in endPostions)
            {
                endCells.Add(cells[endPostion.y*width+endPostion.x]);
            }
                
            stepStack = new List<Cell>();
            stepCurrent = startCell;
            
            stepStack.Add(startCell);
            stepCurrent.visited = true;
        }
        
        public List<Cell> NextSearch(ref bool over,out Cell stepCell,ref  bool getSolute ,ref List<Cell> stacks)
        {
            getSolute = false;
            over = false;
            var tempStack = stepStack;
            Cell neighbor = GetARandomNeighbor(stepCurrent);
            if (neighbor != null)
            {
                neighbor.visited = true;
                OpenWall(stepCurrent,neighbor);
                tempStack.Add(neighbor);
                stepCurrent = neighbor;
                stepCell = stepCurrent;

                if (endCells.Contains(neighbor))
                {
                    stacks = new List<Cell>();
                    stacks.AddRange(tempStack.ToArray());
                    getSolute = true;
                    
                    stepCurrent = tempStack[tempStack.Count-2];
                    tempStack.RemoveAt(tempStack.Count-1);
                    stepCell = stepCurrent;
                }
            }
            else if(tempStack.Count > 2)
            {
                stepCurrent = tempStack[tempStack.Count-2];
                tempStack.RemoveAt(tempStack.Count-1);
                stepCell = stepCurrent;
            }
            else
            {
                over = true;
                stepCell = stepCurrent;
            }
            
            return cells;
        }

        

        public void BeginMakeMaze()
        {
            foreach (Cell cell in cells)
            {
                cell.Reset();
            }
            stepStack = new List<Cell>();
            stepCurrent = cells[0];
            stepStack.Add(cells[0]);
            stepCurrent.visited = true;
        }

        public List<Cell> NextStep(ref bool over,out Cell stepCell)
        {
            over = false;
            var tempStack = stepStack;
            Cell neighbor = GetARandomNeighbor(stepCurrent);
            if (neighbor != null)
            {
                neighbor.visited = true;
                OpenWall(stepCurrent,neighbor);
                tempStack.Add(neighbor);
                stepCurrent = neighbor;
                stepCell = stepCurrent;
            }
            else if(tempStack.Count > 2)
            {
                stepCurrent = tempStack[tempStack.Count-1];
                tempStack.RemoveAt(tempStack.Count-1);
                stepCell = stepCurrent;
            }
            else
            {
                over = true;
                stepCell = stepCurrent;
            }

            return cells;
        }
			
        public List<Cell> RemakeMaze()
        {
            foreach (Cell cell in cells)
            {
                cell.Reset();
            }
            
            List<Cell> tempStack = new List<Cell>();
            Cell currentCell = cells[0];
            tempStack.Add(cells[0]);
            currentCell.visited = true;

            while (true)
            {
                Cell neighbor = GetARandomNeighbor(currentCell);
                if (neighbor != null)
                {
                    neighbor.visited = true;
                    OpenWall(currentCell,neighbor);
                    tempStack.Add(neighbor);
                    currentCell = neighbor;
                }
                else if(tempStack.Count > 2)
                {
                    currentCell = tempStack[tempStack.Count-1];
                    tempStack.RemoveAt(tempStack.Count-1);
                }
                else
                {
                    break;
                }    
            }
            
            return cells;
        }

        private Cell GetARandomNeighbor(Cell current)
        {
            int x = current.x;
            int y = current.y;
            List<Cell> neighborhood = new List<Cell>();
            if (x > 0) //left
            {
				Cell left = cells[y*width+x-1];
                if (left.visited == false)
                {
                    neighborhood.Add(left);
                }
            }

            if (x < width - 1)
            {
				Cell right = cells[y*width + x+1];
                if (right.visited == false)
                {
                    neighborhood.Add(right);
                }
            }

            if (y > 0) //up
            {
				Cell up = cells[(y-1)*width+x];
                if (up.visited == false)
                {
                    neighborhood.Add(up);
                }
            }

            if (y < height - 1)
            {
				Cell down = cells[(y+1)*width + x];
                if (down.visited == false)
                {
                    neighborhood.Add(down);
                }
            }

            if (neighborhood.Count == 0)
            {
                return null;
            }
            else
            {
                int rid = UnityEngine.Random.Range(0,neighborhood.Count);
                return neighborhood[rid];
            }
        }

        private void OpenWall(Cell current ,Cell neighbor)
        {
            int currentX = current.x;
            int currentY = current.y;
            int neighborX = neighbor.x;
            int neighborY = neighbor.y;

            if (neighborX > currentX)
            {
                current.right = true;
                neighbor.left = true;
            }

            if (neighborX < currentX)
            {
                current.left = true;
                neighbor.right = true;
            }

            if (neighborY > currentY)
            {
                current.up = true;
                neighbor.down = true;
            }

            if (neighborY<currentY)
            {
                current.down = true;
                neighbor.up = true;
            }

        }

		//获取一个节点附近的点
		public List<Cell> GetNeighbor(Cell current)
		{
			int x = current.x;
			int y = current.y;
			List<Cell> neighborhood = new List<Cell>();
			if (x > 0) //left
			{
				Cell left = cells[y*width+x-1];

				neighborhood.Add(left);
				
			}

			if (x < width - 1)
			{
				Cell right = cells[y*width + x+1];
				neighborhood.Add(right);
			}

			if (y > 0) //up
			{
				Cell up = cells[(y-1)*width+x];
				neighborhood.Add(up);
			}

			if (y < height - 1)
			{
				Cell down = cells[(y+1)*width + x];
				neighborhood.Add(down);
			}

			return neighborhood;
		}

	

		//删掉路径的回路
		public List<Cell> RemoveCloseRote(List<Cell> rote)
		{
			List<Cell> newRote = new List<Cell> ();
			newRote.AddRange (rote.ToArray());

			for (int i = 0; i < newRote.Count; i++) {
				List<Cell> neighbor = this.GetNeighbor (newRote[i]);
				bool cut = false;
				int cutID = 0;
				for (int j = 0; j < i; j++) {
					var current = newRote[j];
					if (neighbor.Contains (current)) {
						cut = true;
						cutID = j;
						break;
					}
				}

				if (cut == true) {
					for(int k =cutID+1;k<i;k++){
						newRote.RemoveAt (k);
						k--;
						i--;
					}
				}
			}
		
			return newRote;
		}
        
    }
    
}

