import csv
import matplotlib.pyplot as plt
from sklearn import preprocessing
import numpy as np


class Slot:
    def __repr__(self) -> str:
        return (f"x1: {self.start}, x2: {self.end}, distance: {self.distance}, intensity: {self.intensity}")

    def __init__(self, start, end, distance, intensity: list) -> None:
        self.start = start
        self.end = end
        self.distance = distance
        self.intensity = intensity


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


def write_to_file(filename_out: str, list_of_filtered_data: list):
    with open(filename_out, 'w') as out:
        csv_out = csv.writer(out)
        for row in list_of_filtered_data:
            csv_out.writerow(row)
        out.close()


def create_slots_from_coordinates(coordinates: list, threshold: float) -> "dict":
    '''
    checks if amino acids fits between two or more slots
    create Slot object and return a dictionary of amino acids and its valid slots
    '''
    acid_to_slots = {}
 
    all_slots = make_slot_objects(coordinates)
    for acid in amino_acids.keys():
        matching_slots = get_all_matching_slots(amino_acids.get(acid), all_slots, threshold)
        acid_to_slots[acid] = matching_slots
    return acid_to_slots
        
def get_all_matching_slots(acid, all_slots, threshold):
    '''
    checks if amino acid +/- threshold fits in slot
    returns list of all valid slots
    '''
    valid_slots = []
    for slot in all_slots:
        if slot.distance - threshold <= acid <= slot.distance + threshold:
            valid_slots.append(slot)
    return valid_slots


def make_slot_objects(coordinates: list):
    # Slot width == distance between peaks
    slots = []
    for i, (first_valueX, first_valueY) in enumerate(coordinates):
        for(second_valueX, second_valueY) in coordinates[i+1:]:
            slot_width = second_valueX - first_valueX
            slot = Slot(first_valueX, second_valueX, slot_width, [first_valueY, second_valueY])
            slots.append(slot)
    return slots
 

def filter_on_percentage(coordinates: list, threshold_percentage: float) -> "list[tuple]":
    '''returns a list of coordinates where the intensity of the peaks are over the chosen threshold'''
    valid_coord = []
    y_coord = get_y_coord(coordinates)

    for (x, y) in coordinates:
        if percentage(y, max(y_coord)) >= threshold_percentage:
            valid_coord.append((x, y))
    return valid_coord


def normalize_data(coordinates: list):  # not used yet
    x_array = np.array(get_x_coord(coordinates))
    normalized_x_arr = preprocessing.normalize([x_array])
    return normalized_x_arr


def percentage(part: float, whole: float) -> float:
    return (part/whole*100)


def get_x_coord(coordinates: list) -> "list[float]":
    return [x[0] for x in coordinates]


def get_y_coord(coordinates: list) -> "list[float]":
    return [y[1] for y in coordinates]


def main():
    coordinates = read_file('Assets/Scripts/selected_spectra.csv')
    coordinates = filter_on_percentage(coordinates, 20)
    filtered_on_amino_acids: list[Slot] = create_slots_from_coordinates(
        coordinates, 0.02)

    print(filtered_on_amino_acids)
    #print(map_amino_acid_to_slot_numbers(filtered_on_amino_acids))

    print(f"Number of peaks before filtering: {len(coordinates)} \nNumber of peaks after filtering on percentage: {len(coordinates)}\nNumber of peaks after filtering on percentage and amino acids: {len(filtered_on_amino_acids)}")
    # print(map_amino_acid_to_slot_numbers(filtered_on_amino_acids))

    # plot graph
    """   plt.bar(get_x_coor(coordinates), get_y_coord(coordinates))
    plt.xlabel('m/z')
    plt.ylabel('int') """
    # plt.show()

    """    plt.bar(get_x_coord(filtered_on_amino_acids),
            get_y_coord(filtered_on_amino_acids))
    plt.xlabel('m/z')
    plt.ylabel('int') """
    # plt.show()

    #write_to_file('Assets/Scripts/filtered_data.csv', filtered_on_amino_acids)


main()
