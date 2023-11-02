import random

def generate_maze(width, height):
    # Create a grid with all walls
    maze = [[1 for _ in range(width)] for _ in range(height)]
    
    def dfs(x, y):
        # Mark the current cell as visited
        maze[y][x] = 0
        
        # Define the directions (up, down, left, right)
        directions = [(0, -2), (0, 2), (-2, 0), (2, 0)]
        random.shuffle(directions)
        
        # Visit neighboring cells
        for dx, dy in directions:
            new_x, new_y = x + dx, y + dy
            if 0 <= new_x < width and 0 <= new_y < height and maze[new_y][new_x] == 1:
                # Remove the wall between the current cell and the neighbor
                maze[y + dy // 2][x + dx // 2] = 0
                dfs(new_x, new_y)
    
    # Start DFS from a random initial cell
    start_x, start_y = random.randrange(1, width, 2), random.randrange(1, height, 2)
    dfs(start_x, start_y)
    
    return maze

# Example usage
width, height = 21, 21  # Adjust the size of the maze as needed
maze = generate_maze(width, height)

# Print the generated maze
for row in maze:
    print(' '.join(['#' if cell == 1 else ' ' for cell in row]))
