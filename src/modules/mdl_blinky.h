
#include <Arduino.h>

void blinky(int ledPin, int n){
        
    pinMode(ledPin, OUTPUT);
    Serial.printf("blinky: %i ...\n", n );
   
    for(int i = 0; i < n; i++){
        digitalWrite(ledPin, HIGH);
        delay(500);
        digitalWrite(ledPin, LOW);
        delay(500);
    }
}