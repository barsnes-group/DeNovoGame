import csv
import matplotlib.pyplot as plt
from sklearn import preprocessing
import numpy as np

amino_acids = {
    "A": 71.037114,
    "R": 156.101111,
    "N": 114.042927,
    "D": 115.026943,
    "C": 103.009185,
    "E": 129.042593,
    "Q": 128.058578,
    "G": 57.021464,
    "H": 137.058912,
    "I": 113.084064,
    "L": 113.084064,
    "K": 128.094963,
    "M": 131.040485,
    "F": 147.068414,
    "P": 97.052764,
    "S": 87.032028,
    "T": 101.047679,
    "U": 150.95363,
    "W": 186.079313,
    "Y": 163.06332,
    "V": 99.068414
}


def read_file(file_in):
    coord = []
    with open(file_in, 'r') as csvfile:
        reader = csv.reader(csvfile, delimiter=' ')
        for row in reader:
            tuple = (float(row[0]), float(row[1]))
            coord.append(tuple)
    return coord


def write_to_file(filename_out, list_of_filtered_data: list):
    with open(filename_out, 'w') as out:
        csv_out = csv.writer(out)
        for row in list_of_filtered_data:
            csv_out.writerow(row)
        out.close()


def filter_amino_acid(coordinates: list, threshold: float):
    '''
    filter a list of coordinates so only the coordinates where the mass 
    of an amino acid fits inbetween is kept
    '''
    valid_coord = []
    for i, (first_valueX, first_valueY) in enumerate(coordinates):
        for(second_valueX, second_valueY) in coordinates[i+1:]:
            distance = second_valueX - first_valueX
            # if int(distance) in amino_acids.values():
            if dist_in_range(distance, threshold, amino_acids):
                valid_coord.append((first_valueX, first_valueY))
                valid_coord.append((second_valueX, second_valueY))
                # print(distance, "x1 ", first_valueX, "x2: ", second_valueX)

    # remove duplicates
    valid_coord = list(dict.fromkeys(valid_coord))
    return valid_coord


def filter_on_percentage(coordinates: list, threshold_percentage: float):
    valid_coord = []
    y_coord = get_y_coord(coordinates)

    for (x, y) in coordinates:
        if percentage(y, max(y_coord)) >= threshold_percentage:
            valid_coord.append((x, y))
    return valid_coord


def normalize_data(coordinates: list):
    x_array = np.array(get_x_coor(coordinates))
    normalized_arr = preprocessing.normalize([x_array])
    return normalized_arr


def dist_in_range(input_number: float, threshold: float, amino_acid: list):
    list_of_numbers_in_range = []

    # loop amino_acids
    for x in amino_acid.values():
        if input_number-threshold <= x <= input_number+threshold:
            list_of_numbers_in_range.append((input_number-threshold)+threshold)

    return list_of_numbers_in_range


def percentage(part: float, whole: float):
    return (part/whole*100)


def get_x_coor(coordinates: list):
    return [x[0] for x in coordinates]


def get_y_coord(coordinates: list):
    return [y[1] for y in coordinates]


def main():
    coordinates = read_file('Assets/Scripts/selected_spectra.csv')
    filtered_on_percentage = filter_on_percentage(coordinates, 5)
    filtered_on_amino_acids = filter_amino_acid(filtered_on_percentage, 0.02)

    print(
        f"Number of peaks before filtering: {len(coordinates)} \nNumber of peaks after filtering: {len(filtered_on_amino_acids)}")

    # plot graph
    plt.bar(get_x_coor(coordinates), get_y_coord(coordinates))
    plt.xlabel('m/z')
    plt.ylabel('int')
    # plt.show()

    plt.bar(get_x_coor(filtered_on_amino_acids),
            get_y_coord(filtered_on_amino_acids))
    plt.xlabel('m/z')
    plt.ylabel('int')
    # plt.show()

    write_to_file('Assets/Scripts/filtered_data.csv', filtered_on_amino_acids)

main()
