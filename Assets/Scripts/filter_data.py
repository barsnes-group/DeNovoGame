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

    amino_acid(mz)

    print(f"Number of peaks: {len(mz)}")

    # plot graph
    plt.bar(mz, int_graph)
    plt.xlabel('m/z')
    plt.ylabel('int')
    plt.legend()
    # plt.show()

    # write to file
    '''
    with open(filename_out, 'w') as csvfile:
        writer = csv.writer(csvfile)
        
        for w in range(len(mz)):
            writer.writerow([mz[w], int_graph[w]])
        csvfile.close()
    '''


def amino_acid(list_of_peaks):
    amino_acids = {
        "N": 114,
        "V": 99
    }
   
    n = 10;
    while n > 0:
        for peak in range(len(list_of_peaks)-n):
            distance = round((list_of_peaks[peak+n] - list_of_peaks[peak]),0)
            
            if distance >=56 and distance <= 186:
                if distance in amino_acids.values():
                    print(f"{distance} in dict")
                else:
                    print(f"{distance} not in dict")
        n-=1
        print("NEW LOOP")
           
            
            
  







read_write_file('Assets/Scripts/selected_spectra.csv', ' ', 0)
