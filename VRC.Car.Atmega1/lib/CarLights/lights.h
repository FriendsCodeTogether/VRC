/*
 * lights.h
 *
 * Created: 12/11/2020 0:33:37
 *  Author: VRC-Team
 */

#ifndef LIGHTS_H_
#define LIGHTS_H_

#define LeftIndicator 0
#define RightIndicator 1
#define FrontBarRight 2
#define FrontBarMiddleRight 3
#define FrontBarMiddleLeft 4
#define FrontBarLeft 5
#define FrontRight 6
#define FrontLeft 7

void lightsSetup(void);

void lightsSetLed(int led);

void lightsClearLed(int led);

void lightsToggleLed(int led);

void lightsClearAllLeds(void);

void lightsClearIndicators(void);

void lightsClearFrontBar(void);

void lightsClearPositionLights(void);

void lightsFrontBar(void);

void lightsPositionLights(void);

void lightsLeftIndicator(void);

void lightsRightIndicator(void);

void lightsTest(void);

#endif /* LIGHTS_H_ */
