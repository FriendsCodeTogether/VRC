/*
* L298HN.c
*
* Created: 2/11/2020 17:02:03
*  Author: VRCar-Team
*/
#include "l298hn.h"
#include <avr/io.h>
#include "pwm.h"

void HBridgeSetup(void)
{
    DDRB = 0xff;
    DDRD = DDRD | (1 << rightWheelsEnable);
    pwmSetup();
}

void driveCar(char direction, char throttle, int speed)
{
    pwmSpeed(speed);

    if (throttle == 'N')
    {
        stopCar();
    }
    else if (direction == 'N' && throttle == 'F')
    {
        driveStraightForwards();
    }
    else if (direction == 'N' && throttle == 'B')
    {
        driveStraightBackwards();
    }
    else if (direction == 'L' && throttle == 'F')
    {
        driveLeftForwards();
    }
    else if (direction == 'L' && throttle == 'B')
    {
        driveLeftBackwards();
    }
    else if (direction == 'R' && throttle == 'F')
    {
        driveRightForwards();
    }
    else if (direction == 'R' && throttle == 'B')
    {
        driveRightBackwards();
    }
}

void stopCar(void)
{
    PORTB = PORTB & ~(1 << leftWheelsForwards);
    PORTB = PORTB & ~(1 << rightWheelsForwards);

    PORTB = PORTB & ~(1 << leftWheelsBackwards);
    PORTB = PORTB & ~(1 << rightWheelsBackwards);

    PINB = PINB & ~(1 << leftWheelsEnable);
    PIND = PIND & ~(1 << rightWheelsEnable);
}

void driveStraightForwards(void)
{
    PORTB = PORTB | (1 << leftWheelsForwards);
    PORTB = PORTB | (1 << rightWheelsForwards);

    PORTB = PORTB & ~(1 << leftWheelsBackwards);
    PORTB = PORTB & ~(1 << rightWheelsBackwards);

    PINB = PINB | (1 << leftWheelsEnable);
    PIND = PIND | (1 << rightWheelsEnable);
}

void driveStraightBackwards(void)
{
    PORTB = PORTB & ~(1 << leftWheelsForwards);
    PORTB = PORTB & ~(1 << rightWheelsForwards);

    PORTB = PORTB | (1 << leftWheelsBackwards);
    PORTB = PORTB | (1 << rightWheelsBackwards);

    PINB = PINB | (1 << leftWheelsEnable);
    PIND = PIND | (1 << rightWheelsEnable);
}

driveLeftForwards()
{
    PORTB = PORTB & ~(1 << leftWheelsForwards);
    PORTB = PORTB | (1 << rightWheelsForwards);

    PORTB = PORTB & ~(1 << leftWheelsBackwards);
    PORTB = PORTB & ~(1 << rightWheelsBackwards);

    PINB = PINB & ~(1 << leftWheelsEnable);
    PIND = PIND | (1 << rightWheelsEnable);
}

driveLeftBackwards()
{
    PORTB = PORTB & ~(1 << leftWheelsForwards);
    PORTB = PORTB & ~(1 << rightWheelsForwards);
    PORTB = PORTB & ~(1 << leftWheelsBackwards);
    PORTB = PORTB | (1 << rightWheelsBackwards);
    PINB = PINB & ~(1 << leftWheelsEnable);
    PIND = PIND | (1 << rightWheelsEnable);
}

driveRightForwards()
{
    PORTB = PORTB | (1 << leftWheelsForwards);
    PORTB = PORTB & ~(1 << rightWheelsForwards);
    PORTB = PORTB & ~(1 << leftWheelsBackwards);
    PORTB = PORTB & ~(1 << rightWheelsBackwards);
    PINB = PINB | (1 << leftWheelsEnable);
    PIND = PIND & ~(1 << rightWheelsEnable);
}

driveRightBackwards()
{
    PORTB = PORTB & ~(1 << leftWheelsForwards);
    PORTB = PORTB & ~(1 << rightWheelsForwards);
    PORTB = PORTB | (1 << leftWheelsBackwards);
    PORTB = PORTB & ~(1 << rightWheelsBackwards);
    PINB = PINB | (1 << leftWheelsEnable);
    PIND = PIND & ~(1 << rightWheelsEnable);
}
