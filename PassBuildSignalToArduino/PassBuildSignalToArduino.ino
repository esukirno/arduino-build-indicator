#include <Scheduler.h>

const int brokenBuildPin = 13;
const int inProgressBuildPin = TODO;

void setup() {
  Serial.begin(9600);
  pinMode(brokenBuildPin, OUTPUT);
  pinMode(inProgressBuildPin, OUTPUT);
}

void loop() {
  if (Serial.available() > 0) {
    char value = Serial.read();
    if (value == '1') {
      digitalWrite(brokenBuildPin, LOW);
      digitalWrite(inProgressBuildPin, HIGH);
    } 
    else if (value == '2') {
      digitalWrite(brokenBuildPin, HIGH);
      digitalWrite(inProgressBuildPin, LOW);
    } 
    else {
      digitalWrite(brokenBuildPin, LOW);
      digitalWrite(inProgressBuildPin, LOW);
    }
    Serial.println(value);
  }
  
  delay(5000);
}
