# DiceGame: Fair Random Protocol Implementation

## Overview

This project implements a fair random protocol for a dice game, meeting critical security and fairness criteria. The protocol ensures both the computer and the user contribute equally to each random outcome, preventing cheating and allowing the user to verify the integrity of each result using HMAC cryptography.

## Achievements

### Protocol Implementation

- **HMAC-Based Fairness:**  
  The computer generates a secret key, then a random value, and displays only the HMAC to the user.  
  This ensures the computer cannot change its selection after the user makes their move.

- **User Participation:**  
  The user is prompted to enter their own value after seeing the computer's HMAC, without knowing the computer’s number.

- **Reveal and Verification:**  
  Once the user has entered a value, the computer reveals its number, the secret key, and the sum (modulo the dice sides).  
  This allows the user to verify the HMAC using any external tool, ensuring transparency.

- **No Need for External Apps:**  
  All HMAC calculations and key reveals are handled within the app. The user can verify using any trusted HMAC tool or service.

- **Modular Addition for Uniform Distribution:**  
  The final result depends on both the computer and user’s numbers, calculated via modular addition to guarantee uniform randomness.

- **Multiple Values Per Game:**  
  Each game round involves three independent values per party:
    - One to determine who selects the dice first.
    - One for the user roll.
    - One for the computer roll.
  Each value generation is protected by its own HMAC and secret key.

- **Three Keys and HMACs Per Game:**  
  Every game session includes three separate keys and HMAC values for maximum protocol integrity.

- **Console Table Generation:**  
  The results, including all values, keys, HMACs, and moves, are displayed using a console table for clarity and easy reference.  
  (Uses base class libraries and 3rd-party libraries for table generation.)

### Security and Trust

- **User Can Verify Everything:**  
  All critical protocol data (computer value, secret key, generated HMAC) is revealed after each move so the user can independently verify the HMAC matches the computer’s commitment.

- **No Trust Required:**  
  The protocol is designed so the user does not have to trust the application’s code, as verification is always possible with external tools.

### Technical Compliance

- **Strict Dice Validation:**  
  The dice implementation enforces that each die has between 2 and 20 faces, with checks in both the model and parser for data integrity.

- **Flexible and Extensible:**  
  The design is modular, allowing for future extension and adaptation to other fair random protocols or games.

## How to Use

1. Start the application.
2. Follow the prompts for dice configuration and game rounds.
3. For each move, observe the HMAC before entering your number.
4. After each round, check the revealed computer value and secret key against the original HMAC.
5. Results are displayed in a clear console table for easy verification.

## Why This Matters

This project demonstrates a robust, fair protocol for random number generation in competitive games, using modern cryptographic practices, transparent verification, and user-centered design. Every random outcome is the product of two independent choices, verifiable by both parties, ensuring trust and fairness.
