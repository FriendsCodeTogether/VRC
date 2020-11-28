#include <avr/io.h>
#include <lights.h>
#include <debugBoard.h>

void debugMode(void)
{
  debugBoardSetup();
}

void carSetup(void)
{
  lightsSetup();
}

void startup(void)
{
  lightsTest();
}

int main(void)
{
  //debugMode();
  carSetup();
  startup();

  while (1)
  {
  }
}
