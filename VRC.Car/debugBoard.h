/*
 * debugBoard.h
 *
 * Created: 11/11/2020 17:37:14
 *  Author: VRCar-Team
 */ 


#ifndef DEBUGBOARD_H_
#define DEBUGBOARD_H_

#define SW0 0
#define SW1 1
#define SW2 2
#define SW3 3
#define LedRed 4
#define LedGreen 5
#define LedBlue 6
#define LedWhite 7

void debugBoardSetup(void);

void debugBoardUpdate(void);

int debugBoardButtonPressed(int button);

int debugBoardButtonIsBeingPressed(int button);

void debugBoardShowBinaryNumber(int number); // shows a number in a binary way on the leds

void debugBoardSetLed(int led);

void debugBoardClearLed(int led);

void debugBoardToggleLed(int led);

void debugBoardClearAllLeds(void);

#endif /* DEBUGBOARD_H_ */