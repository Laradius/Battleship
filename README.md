# Battleship

## Description:

Battleship simulation between randomly shooting battleships.

Press any key to proceed to next turn.

## Changelog:

## 1.0
* Code Cleanup
* Ships Added
* Shooting logic
* Turn logic
* Win/Lose Conditions
* Bug fixes where ships could stack on each other



### 0.7
Todo: Add remaining ships, implement remaining shooting logic

* Major GameManager functionallity change (Adding ships that cannot collide with each other)
* Rewrote logic for drawing worlds, code cleanup
* Changes in ShipPart SetPositon so it points to GameManeger world position


### 0.5
Todo: Modify adding ships by free positions so that they cannot neighbour with each other.

* More GameManager functionallity (Adding ships by free position)
* Minor Ship, ShipPart and Position changes
* Added Grid class
* Added PatrolBoat class
* Added CreationOrientation enum

### 0.3
* More GameManager functionallity (creating worlds, drawing worlds)
* Minor ShipPart and Position changes


### 0.2
* Added static GameManager class with basic functionallity
* Added ShipPart class and removed Position from Ship class
* Changed Position to class instead of struct


### 0.1
* Added Position struct
* Added Abstract class Ship with basic functionallity
