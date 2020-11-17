/*
 * hBridge.h
 *
 * Created: 2/11/2020 17:02:31
 *  Author: VRCar-Team
 */ 


#ifndef HBRIDGE_H_
#define HBRIDGE_H_

#define forwards 1
#define backwards 0
#define left 7
#define right 6

void hBridgeSetup(void);

void driveCar(int direction, int throttle, int driveSpeed, int turnSpeed);

#endif /* HBRIDGE_H_ */