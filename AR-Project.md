# AR Project

## Screens

1. Loading Screen
2. 1v1 or 1vCPU select screen (Optional, default 1vCPU)
3. Fighter select screen
4. Fighting Screen
    1. Find Spawn positions and lock
    2. Spawn selected characters (grow animation)
    3. Start Fight
5. Show Win/Lose

- Restart to screen 3/2 or
- Quit

https://youtube.com/shorts/Ff89dnvKuD8?si=yK0LbVBrbgd-GpMM

## Classes

- BattleSystem
- Pokemon/Digimon Controller (Common class)
- HUD
    - (Pokemon/Digimon)SelectorHud
    - BattleHud
        - HPBar
        - AttackButton
    - WinHud
    - LoseHud

## Battle System

Damage = ((Attacker.Attack _ (1.0 + Attacker.Buffs)) _ Attack.Damage) / Enemy.Defense

### Attack

- (Maybe) Attack Points (AP) - 5 AP reload 1 per turn
- Range/Physical Attack

## Digimons

### Victory Greymon

- Stats
    - HP: 5720
    - Attack: 3520
    - Defense: 1339
    - Speed: 27
- Attacks
    - Dramon Breaker (Animation 1/25): 320% (Increases Attack by 50%, for 2 turns)
    - Trident Gaia: 460% (Enemy: -30% Attack, 60% chance stun for 2 turns)
    - Victory Charge
    - Victory Shield
- States/Animation
    - Entrance: in
    - Idle: stand
    - Hit: hurt1
    - Basic Attack: skill2_move_to (physical)
    - Special Attack: skill1_move_to (physical)
    - Jump Back: skill2_move_back + skill1_move_back
    - Victory: Animation 20
          <!-- - Runnning: Animation 22/(Maybe 18) -->
          <!-- - Stunned: Animation 11 -->
          <!-- - Block: Animation 12 -->

### Gallantmon

- Stats
    - HP: 5720
    - Attack: 4309
    - Defense: 1339
    - Speed: 27
- Attack
    - Royal Saber (Animation 22)
        - 210% (Increases Attack by 40%, for 2 turns)
        - Animation Duration: 172
    - Final Elysium (Animation 1): 460% (Shield up to 4200 Damage for 5 turns)
        - Animation Duration: 109
- States/Animation
    - Entrance: Animation (in)
    - Idle: Animation (stand)
    - Hit: Animation (hurt)
    - Basic Attack:
        - skill1 (range)
        - attack_move_to (physical)
    - Special Attack: skill2 (range)
    - Jump Back: attack_move_back
    - Victory: Animation win2
      <!-- - Block/Blast: Animation 14/15 -->
      <!-- - Runnning: Animation (move2) -->
      <!-- - Stunned: Animation 17 -->

### Chaos Gallantmon

- Stats
    - HP: 6289
    - Attack: 4309
    - Defense: 1613
    - Speed: 27
- Attack
    - Chaos Crusher/Shield Destructor (Animation 4/skill1): 210% (Enemy: 40% chance sleep for 2 turns)
    - Demons Disaster (Animation 2/skill2_move_to): 280% (Short Range)
- States/Animation
    - Entrance: in
    - Idle: stand
    - Hit: hurt
    - Basic Attack: skill1 (range)
    - Special Attack: skill2_move_to (physical)
    - Jump Back: skill2_move_back
    - Victory: win
        <!-- - Block/Blast: Animation 14/15 -->
        <!-- - Runnning: Animation (move) -->

### Imperialdramon (Fighter Mode)

- Stats
    - HP: 6870
    - Attack: 4671
    - Defense: 1807
    - Speed: 27
- Attack
    - Imperial Claw (Animation 4): 210% (Increases Attack by 40%, for 2 turns)
    - Positron Laser (Animation 10): 460% (2x on Crit)
- States
    - Idle: Animation 8
    - Hit: Animation 4
    - Blast: Animation 10
    - Death: Animation 5
    - Block: Animation 7
    - Transcendent Positron Laser: Animation 11
