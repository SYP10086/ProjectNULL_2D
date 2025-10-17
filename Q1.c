#include <stdio.h>
#include <stdlib.h>

typedef struct geyserData {
    int cycle;
    int pressure;
    struct geyserData* next;
} GeyserData;

GeyserData* geyserEmit(int* pressures, int threshold) {
    int stack[1000];
    int stackTop = -1;
    int totalPressure = 0;
    int cycleCount = 1;
    GeyserData* head = NULL;
    GeyserData* tail = NULL;

    int i = 0;
    while (pressures[i] != -1) {
        int currentPressure = pressures[i];
        stack[++stackTop] = currentPressure;
        totalPressure += currentPressure;

        while (totalPressure > threshold) {
            int emittedPressure = stack[stackTop--];
            totalPressure -= emittedPressure;

            GeyserData* newNode = (GeyserData*)malloc(sizeof(GeyserData));
            newNode->cycle = -cycleCount;
            newNode->pressure = emittedPressure;
            newNode->next = NULL;

            if (head == NULL) {
                head = newNode;
                tail = newNode;
            }
            else {
                tail->next = newNode;
                tail = newNode;
            }
        }

        GeyserData* newNode = (GeyserData*)malloc(sizeof(GeyserData));
        newNode->cycle = cycleCount;
        newNode->pressure = totalPressure;
        newNode->next = NULL;

        if (head == NULL) {
            head = newNode;
            tail = newNode;
        }
        else {
            tail->next = newNode;
            tail = newNode;
        }

        cycleCount++;
        i++;
    }

    return head;
}

void printResult(GeyserData* head) {
    GeyserData* current = head;
    while (current != NULL) {
        printf("{%d,%d} ", current->cycle, current->pressure);
        current = current->next;
    }
    printf("\n");
}

void freeResult(GeyserData* head) {
    GeyserData* current = head;
    while (current != NULL) {
        GeyserData* temp = current;
        current = current->next;
        free(temp);
    }
}

int main() 
    {
    int pressures[100];
    int pressure, i = 0;

    printf("pressures = ");
    while (1) {
        scanf_s("%d", &pressure);
        if (pressure == -1) {
            pressures[i] = pressure;
            break;
        }
        pressures[i++] = pressure;
    }

    int threshold;
    printf("threshold = ");
    scanf_s("%d", &threshold);

    GeyserData* result = geyserEmit(pressures, threshold);

    printResult(result);

    freeResult(result);

    return 0;
}