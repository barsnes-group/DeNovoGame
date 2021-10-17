public class peaks{

    
    public static void main(String[] args) {
        List<Double> liste = new ArrayList<>(1.0,2.0,3.0,4.0,5.0,6.0,7.0,8.0,9.0,10.0);
        percentileSorted(liste, 0.25);
    }
    /**
     * Returns the desired percentile in a list of sorted double values. If the
     * percentile is between two values a linear interpolation is done. The list
     * must be sorted prior to submission.
     *
     * @param input the input list
     * @param percentile the desired percentile. 0.01 returns the first
     * percentile. 0.5 returns the median.
     *
     * @return the desired percentile
     */
    public static double percentileSorted(ArrayList<Double> input, double percentile) {
        if (percentile < 0 || percentile > 1) {
            throw new IllegalArgumentException(
                    "Incorrect input for percentile: "
                    + percentile + ". Input must be between 0 and 1.");
        }
        if (input == null) {
            throw new IllegalArgumentException(
                    "Attempting to estimate the percentile of a null object.");
        }
        int length = input.size();
        if (length == 0) {
            throw new IllegalArgumentException(
                    "Attempting to estimate the percentile of an empty list.");
        }
        if (length == 1) {
            return input.get(0);
        }
        double indexDouble = percentile * (length - 1);
        int index = (int) (indexDouble);
        double valueAtIndex = input.get(index);
        double rest = indexDouble - index;
        if (index == input.size() - 1 || rest == 0) {
            return valueAtIndex;
        }
        return valueAtIndex + rest * (input.get(index + 1) - valueAtIndex);
    }

}