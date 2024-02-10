# 3D Procedural Animation Spider Project

## Overview
This are the code and Assets for the procedural animation technique for a spider model. The spider's legs adjust in real-time to the ground beneath them, which lets the movement adjust to uneven terrain.

## Key Features

- **Procedural Leg Animation**: Utilizes inverse kinematics (IK) to animate each of the spider's legs, allowing them to adapt to the terrain beneath them accurately.
- **Dynamic Body Adjustment**: The spider's body height and orientation adjust based on the positioning of its legs, ensuring the body responds naturally to the terrain it traverses.
- **Diagonal Leg Movement**: Legs move in diagonal pairs to mimic the natural gait of a spider, enhancing the realism of the animation.
- **Raycast-Based Terrain Detection**: Uses raycasting to dynamically detect the terrain beneath each leg, determining the next step position based on the terrain's contour.

## Implementation Details

### Inverse Kinematics (IK)
Calculates the Position of each bone depending on the Target position.

### Raycasting for Terrain Adaptation
To determine where each leg should step next, the system casts a ray downward from the expected next step position. The point where this ray intersects with the terrain dictates the next target position for the leg.

### Body Position and Rotation Adjustment
The spider's body position and orientation are dynamically adjusted based on the average position of all leg endpoints.

### Diagonal Leg Movement Coordination
Legs are grouped into two sets, moving in diagonal pairs.

## How to Use

1. A full body with legs can be found in /Assets/Prefabs.
2. Add the Target Spheres (also found in /Assets/Prefabs to the ground at the bottom of the legs and add them to the array of the "IK.cs" Script on the last bone of each Leg, in the inspector.
3. Use WASD to Move

## Picture of Spider on a sphere
![Spider_screenshot](https://github.com/Tr0sh55/3D_Procedural_Animation/assets/47827386/7a1d6589-d3aa-435e-b049-43af21ff7980)

