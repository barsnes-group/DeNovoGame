import csv
from os import write
import matplotlib.pyplot as plt

def percentage(part : float, whole : float):
    return 100 * part/whole

mz = []
intensity = []
int_graph = []
filtered_graph = []

with open('code/selected_spectra.csv', 'r') as csvfile:
    reader = csv.reader(csvfile, delimiter=' ')
    for row in reader:  
        intensity.append(float(row[1]))
        
        max_intensity = max(intensity)
        threshold_percentage = (float(row[1])/max_intensity*100)
        
        if threshold_percentage >= 5:
            mz.append(float(row[0]))
            int_graph.append(float(row[1]))   

            
plt.bar(mz, int_graph)
plt.xlabel('m/z')
plt.ylabel('int')

plt.legend()
#plt.show()


with open('test.csv', 'w') as csvfile:
    writer = csv.writer(csvfile)
    
    for w in range(len(mz)):
        writer.writerow([mz[w], int_graph[w]])
    csvfile.close()