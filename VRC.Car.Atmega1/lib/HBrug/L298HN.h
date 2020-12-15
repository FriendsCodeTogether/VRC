#ifndef L298HN_H_
#define L298HN_H_

#define leftWheelsForwards 1
#define leftWheelsBackwards 0
#define rightWheelsForwards 4
#define rightWheelsBackwards 2

#define leftWheelsEnable 3
#define rightWheelsEnable 7 //PORTD

void HBridgeSetup(void);
void driveCar(char direction, char throttle, int speed);

#endif /* L298HN_H_ */