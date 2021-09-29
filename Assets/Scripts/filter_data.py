import csv
import matplotlib.pyplot as plt

amino_acids = {
    "N": 114,
    "V": 99
}

def read_file(file_in):
    coord = []
    with open(file_in, 'r') as csvfile:
        reader = csv.reader(csvfile, delimiter=' ')
        for row in reader:
            tuple = (float(row[0]), float(row[1]))
            coord.append(tuple)
    return coord


def write_to_file(filename_out):
    with open(filename_out, 'w') as csvfile:
        writer = csv.writer(csvfile)

        for w in range(len(mz)):
            writer.writerow([mz[w], int_graph[w]])
        csvfile.close()


def normalize_data():
    pass


def filter_amino_acid(coordinates):
    valid_coor = []
    for i, (first_valueX, first_valueY) in enumerate(coordinates):
        for( second_valueX, second_valueY) in coordinates[i+1:]:
            distance = second_valueX - first_valueX
            if int(distance) in amino_acids.values():
                valid_coor.append((first_valueX, first_valueY))
                valid_coor.append((second_valueX, second_valueY))
    # remove duplicates
    valid_coor = list(dict.fromkeys(valid_coor))
    return valid_coor




def filter_on_percentage(coord):
    for pos in coord:

        intensity = pos[1]
        # calculate percentage
        max_intensity = max(intensity)
        threshold_percentage = (intensity)/max_intensity*100

        if threshold_percentage >= percentage:
            mz.append(float(row[0]))
            int_graph.append(float(intensity))

            filter_amino_acid(mz)

        print(f"Number of peaks: {len(mz)}")

def getXcoor(coordinates):
    return [x[0] for x in coordinates]

def main():
    coordinates = read_file('Assets/Scripts/selected_spectra.csv')
    filtered_coordintes = filter_amino_acid(coordinates)
    print(filtered_coordintes)
    
main()