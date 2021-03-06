/*
 * lights.h
 *
 * Created: 12/11/2020 0:33:37
 *  Author: VRC-Team
 */

#ifndef LIGHTS_H_
#define LIGHTS_H_

#define BigLights 5
#define BrakeLights 4
#define RearLights 3
#define LeftIndicator 2
#define RightIndicator 1
#define PositionLights 0
#define LDR 2 //PortD

void lightsSetup(void);

void lightsSetLed(int led);

void lightsClearLed(int led);

void lightsToggleLed(int led);

void lightsClear(void);

void ReadLDR(void);

#endif /* LIGHTS_H_ */
