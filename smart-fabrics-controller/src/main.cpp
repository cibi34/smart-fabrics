
#include <Arduino.h>
#include "modules/mdl_blinky.h"
#include "modules/mdl_mpr121.h"


const int BUTTON_PIN = 0;
const int LED_PIN = 5;

void buttonfn();

void setup(void){

  pinMode(BUTTON_PIN, INPUT_PULLUP);
  pinMode(LED_PIN, OUTPUT);

  Serial.begin(115200);
  Serial.println("--- SMART FABRICS ---\n");

  attachInterrupt(0, buttonfn, FALLING);

  mpr_setup();
}


void loop(void){
  
  mpr_loop();
 
}


void buttonfn(){
    Serial.println("Onboard BUTTON pressed...");
}