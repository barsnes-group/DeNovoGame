import csv
import json
import pprint
import os
import matplotlib.pyplot as plt
import argparse

parser = argparse.ArgumentParser()
parser.add_argument("-p", "--percentile",
                    help="Must be a number. Choose the percentile threshold for the intensity of the peaks.", type=float, default=85)
parser.add_argument("-t", "--threshold",
                    help="Must be a number. Choose the threshold for the ", type=float, default=0.02)
args = parser.parse_args()


class Slot:
    def __init__(self, start, end, intensity: list) -> None:
        self.start = start
        self.end = end
        self.intensity = intensity

    def __repr__(self) -> str:
        return (f"peak 1: {self.peak_1}, peak 2: {self.peak_2}, x1: {round(self.start,3)}, x2: {round(self.end,3)}, intensity: {[round(intensity,3) for intensity in self.intensity]}")

    def to_dict(self, peak_to_index):
        new_dict = {
                    "start_peak_index": peak_to_index[self.start], 
                    "start_peak_coord": round(self.start, 3),
                    "end_peak_index": peak_to_index[self.end],
                    "end_peak_coord": round(self.end, 3),
                    "intensity": [round(intensity, 3) for intensity in self.intensity]
                    }
        return new_dict

    def width(self) -> float:
        return abs(self.end - self.start)
        
        
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
    "I/L": 113.084064,
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
        reader = csvfile.readlines()
        for row in reader[5:-1]:
            row = row.strip()
            row = row.split(" ")
            xy = (float(row[0]), float(row[1]))
            coord.append(xy)
    return coord


def playing_board_file(filename_out: str, list_of_filtered_data: list):
    '''
    make file with x- and y-coordinates
    '''
    with open(filename_out, 'w') as out:
        csv_out = csv.writer(out)
        for row in list_of_filtered_data:
            csv_out.writerow(row)
        out.close()


def create_slots_from_coordinates(coordinates: list, threshold: float) -> "dict[str, list[Slot]]":
    '''
    create Slot object
    get all matching slots for each amino acid
    return a dictionary of amino acids and its valid slots
    '''
    amino_acid_to_slots = {}
    all_slots = make_slot_objects(coordinates)
    normalize_amino_acids(max_x_slot(all_slots))
    normalize_slots(all_slots)

    for amino_acid in amino_acids.keys():
        matching_slots = get_all_matching_slots(amino_acids.get(amino_acid), all_slots, threshold)
        amino_acid_to_slots[amino_acid] = matching_slots
    return amino_acid_to_slots


def list_of_Slot_coord(dict_of_Slots) -> "list[tuple]":
    '''
    returns a list of x- and y-coordinates of a Slot
    '''
    coord = []

    for slot_list in dict_of_Slots.values():
        if len(slot_list) != 0:
            for slot in slot_list:
                coord.append(
                    (round(slot.start, 3), round(slot.intensity[0], 3)))
                coord.append((round(slot.end, 3), round(slot.intensity[1], 3)))
    coord = list(dict.fromkeys(coord))
    return coord


def get_all_matching_slots(acid: float, all_slots: "list[Slot]", threshold: float) -> "list[Slot]":
    '''
    checks if amino acid +/- threshold fits in slot
    returns list of all valid slots
    '''
    valid_slots = []
    for slot in all_slots:
        if slot.width() - threshold <= acid <= slot.width() + threshold:
            valid_slots.append(slot)
    return valid_slots


def make_slot_objects(coordinates: list) -> "list[Slot]":
    '''
    create Slot object from list of coordinates
    return list of Slots
    '''
    slots = []
    for i, (first_valueX, first_valueY) in enumerate(coordinates):
        for(second_valueX, second_valueY) in coordinates[i+1:]:
            slot = Slot(first_valueX, second_valueX, [
                        first_valueY, second_valueY])
            slots.append(slot)
    return slots


def percentile_sorted(coordinates: list, threshold_percentage: float) -> "list[tuple]":
    '''
    returns a list of coordinates where the intensity of the peaks are over the chosen threshold
    '''
    valid_coord = []
    # sort coordinates by the y-value
    sorted_coord = sorted(coordinates, key=lambda x: x[1])
    valid_coord = sorted_coord[int(
        len(sorted_coord) * threshold_percentage / 100):]

    return valid_coord


def max_x_slot(slots):
    max_x = -1
    for slot in slots:
        x = max(slot.start, slot.end)
        if max_x < x:
            max_x = x
    return max_x


def max_y_slot(slots):
    max_y = -1
    for slot in slots:
        y = max(slot.intensity)
        if max_y < y:
            max_y = y
    return max_y


def normalize_slots(slots: list[Slot]):
    max_x = max_x_slot(slots)
    max_y = max_y_slot(slots)

    for slot in slots:
        slot.start = slot.start/max_x * 100
        slot.end = slot.end/max_x * 100
        slot.intensity = [y/max_y * 100 for y in slot.intensity]


def normalize_amino_acids(max_x):
    for key in amino_acids.keys():
        value = amino_acids.get(key)
        normalized_value = value/max_x * 100
        amino_acids[key] = normalized_value


def get_x_coord(coordinates: list) -> "list[float]":
    return [x[0] for x in coordinates]


def get_y_coord(coordinates: list) -> "list[float]":
    return [y[1] for y in coordinates]


def plot(input_coord, filtered_coord):
    plt.bar(get_x_coord(input_coord), get_y_coord(input_coord))
    plt.xlabel('m/z')
    plt.ylabel('int')
    plt.show()

    plt.bar(get_x_coord(filtered_coord), get_y_coord(filtered_coord))
    plt.xlabel('m/z')
    plt.ylabel('int')
    plt.show()


def write_to_json(slot_dict: dict, filename: str):
    list_of_amino_acids = []
    peaks_to_index = sorted_peaks(slot_dict)
    for amino_acid, slot in slot_dict.items():
        a_a_dict = {}
        a_a_dict["AminoAcidName"] = amino_acid
        a_a_dict["Mass"] = round(amino_acids[amino_acid], 3)
        slots = []
        for e in slot:
            slots.append(e.to_dict(peaks_to_index))
        a_a_dict["Slots"] = slots
        list_of_amino_acids.append(a_a_dict)
    with open(filename, 'w') as out:
        json.dump(list_of_amino_acids, out)

def sorted_peaks(acid_to_slots: "dict") -> dict:
    '''
    make a list of sorted unique peaks from dictionary of Slots
    return a map where the peak points at an index
    '''
    peaks = []
    for slots in acid_to_slots.values():
        for slot in slots:
            peaks.append(float(slot.start))
            peaks.append(float(slot.end))
    peaks = sorted(peaks)
    #remove duplicates
    peaks = list(acid_to_slots.fromkeys(peaks))
    peaks_index = {}
    for index, peak in enumerate(peaks):
        peaks_index[peak] = index
    return peaks_index
    
    
if __name__ == "__main__":
    cwd = os.getcwd()  # get the current working directory (cwd)
    #coordinates = read_file(f'{cwd}/selected_spectra.mgf')
    coordinates = read_file('Assets/Data/selected_spectra.mgf')
    print(f"Number of peaks before filtering: {len(coordinates)}")
    coordinates = percentile_sorted(coordinates, args.percentile)
    slot_dict = create_slots_from_coordinates(coordinates, args.threshold)
    filtered_Slot_coord = (list_of_Slot_coord(slot_dict))

    print(f"Number of peaks after filtering on percentage: {len(coordinates)}")
    print(f"Number of peaks after filtering on percentage and amino acids: {len(filtered_Slot_coord)}")
    # pprint.pprint(slot_dict)
    # pprint.pprint(filtered_Slot_coord)
    #plot(coordinates, filtered_Slot_coord)
    playing_board_file(f'{cwd}/Assets/Data/playing_board.csv', filtered_Slot_coord)
    write_to_json(slot_dict, f'{cwd}/Assets/Data/aa_to_slots.json')
