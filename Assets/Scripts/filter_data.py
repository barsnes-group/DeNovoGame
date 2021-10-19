import csv
import matplotlib.pyplot as plt

class Slot:
    def __repr__(self) -> str:
        return (f"Slot: x1: {round(self.start,3)}, x2: {round(self.end,3)}, distance: {round(self.width(),3)}, intensity: {[round(intensity,3) for intensity in self.intensity]}")

    def width(self) -> float:
        return abs(self.end - self.start)

    def __init__(self, start, end, intensity: list) -> None:
        self.start = start
        self.end = end
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


def playing_board_file(filename_out: str, list_of_filtered_data: list):
    '''
    make file with x- and y-coordinates
    '''
    with open(filename_out, 'w') as out:
        csv_out = csv.writer(out)
        for row in list_of_filtered_data:
            csv_out.writerow(row)
        out.close()


def create_slots_from_coordinates(coordinates: list, threshold: float) -> "dict[str, Slot]":
    '''
    create Slot object
    get all matching slots for each amino acid
    return a dictionary of amino acids and its valid slots
    '''
    amino_acid_to_slots = {}

    all_slots = make_slot_objects(coordinates)
    for amino_acid in amino_acids.keys():
        matching_slots = get_all_matching_slots(
            amino_acids.get(amino_acid), all_slots, threshold)
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
                coord.append((slot.start, slot.intensity[0]))
                coord.append((slot.end, slot.intensity[1]))
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
    valid_coord = sorted_coord[int(len(sorted_coord) * threshold_percentage / 100):]

    return valid_coord


def normalize_data(coordinates: "list[tuple]") -> "list[tuple]":
    '''
    normalize data by dividing all x-values on max x-value
    '''
    
    norm = []
    max_x = max(get_x_coord(coordinates))
    for (x,y) in coordinates:
        norm.append((x/max_x * 100, y))
    return norm

    #return [x/max_x*100 for (x,y) in coordinates]


def get_x_coord(coordinates: list) -> "list[float]":
    return [x[0] for x in coordinates]


def get_y_coord(coordinates: list) -> "list[float]":
    return [y[1] for y in coordinates]


def main():
    coordinates = read_file('Assets/Scripts/selected_spectra.csv')
    coordinates = percentile_sorted(coordinates, 30)
    filtered_on_amino_acids: dict[Slot] = create_slots_from_coordinates(coordinates, 0.02)
    filtered_Slot_coord: list[tuple] = (list_of_Slot_coord(filtered_on_amino_acids))
    filtered_Slot_coord = normalize_data(filtered_Slot_coord)


    print(f"Number of peaks after filtering on percentage: {len(coordinates)}")
    print(f"Number of peaks after filtering on percentage and amino acids: {len(filtered_Slot_coord)}")    
    
    # plot graph
    min_y = min(get_y_coord(coordinates))
    plt.bar(get_x_coord(coordinates), get_y_coord(coordinates))
    plt.xlabel('m/z')
    plt.ylabel('int')
    plt.axhline(y= min_y, color='r', linestyle='-', label=min_y)
    plt.legend()
    plt.show()
    
    min_y = min(get_y_coord(filtered_Slot_coord))
    plt.bar(get_x_coord(filtered_Slot_coord), get_y_coord(filtered_Slot_coord))
    plt.xlabel('m/z')
    plt.ylabel('int')
    plt.axhline(y= min_y, color='r', linestyle='-', label=min_y)
    plt.legend()
    plt.show()

    # playing_board_file('Assets/Scripts/test.csv', filtered_Slot_coord)

main()
