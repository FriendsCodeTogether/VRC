/*
 * ByteConversions.c
 *
 */

#include "ByteConversions.h"

void intToByteArray(int val, uint8_t* bytesArray)
{
  // Create union of shared memory space
  union {
    int intVariable;
    uint8_t tempArray[2];
  } u;
  // Overite bytes of union with int variable
  u.intVariable = val;
  // Assign bytes to input array
  memcpy(bytesArray, u.tempArray, 2);
}

void longToByteArray(long val, uint8_t* bytesArray)
{
  // Create union of shared memory space
  union {
    long longVariable;
    uint8_t tempArray[4];
  } u;
  // Overite bytes of union with long variable
  u.longVariable = val;
  // Assign bytes to input array
  memcpy(bytesArray, u.tempArray, 4);
}

void floatToByteArray(float val, uint8_t* bytesArray)
{
	// Create union of shared memory space
	union {
		float floatVariable;
		uint8_t tempArray[4];
	} u;
	// Overite bytes of union with float variable
	u.floatVariable = val;
	// Assign bytes to input array
	memcpy(bytesArray, u.tempArray, 4);
}

void doubleToByteArray(double val, uint8_t* bytesArray)
{
	// Create union of shared memory space
	union {
		double doubleVariable;
		uint8_t tempArray[8];
	} u;
	// Overite bytes of union with double variable
	u.doubleVariable = val;
	// Assign bytes to input array
	memcpy(bytesArray, u.tempArray, 8);
}
