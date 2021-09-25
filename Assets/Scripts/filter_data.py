import csv
import matplotlib.pyplot as plt

def read_write_file(file_in, file_out, percentage):
    mz = []
    intensity = []
    int_graph = []

    with open(file_in, 'r') as csvfile:
        reader = csv.reader(csvfile, delimiter=' ')
        for row in reader:  
            intensity.append(float(row[1]))
            
            max_intensity = max(intensity)
            threshold_percentage = (float(row[1])/max_intensity*100)
            
            if threshold_percentage >= percentage:
                mz.append(float(row[0]))
                int_graph.append(float(row[1]))   

                
    print(f"Number of peaks: {len(mz)}")
    
    #plot graph
    plt.bar(mz, int_graph)
    plt.xlabel('m/z')
    plt.ylabel('int')
    plt.legend()
    plt.show()
    
    #write to file
    '''
    with open(filename_out, 'w') as csvfile:
        writer = csv.writer(csvfile)
        
        for w in range(len(mz)):
            writer.writerow([mz[w], int_graph[w]])
        csvfile.close()
    '''
    
    
read_write_file('Assets/Scripts/selected_spectra.csv', ' ', 5)